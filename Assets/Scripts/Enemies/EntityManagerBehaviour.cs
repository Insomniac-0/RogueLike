using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Cinemachine;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.VisualScripting;
using UnityEngine;

[BurstCompile]
public unsafe class EntityManagerBehaviour : MonoBehaviour
{
    //[SerializeField] InputReader input;

    // REFS
    [SerializeField] Enemy enemy_ref;
    [SerializeField] EnemyData[] EnemyDataTemplates;

    float delta_time;

    const int InitialAllocSize = 100;
    const int EntityJobBatchSize = 16;
    // TYPES

    public enum EnemyType
    {
        DEFAULT,
        RANGED,
    }

    // ARRAYS
    NativeList<EntityData> enemies;

    List<Enemy> enemy_objects;
    List<Enemy> enemy_pool;







    float3 mouse_pos;
    float3 player_position;

    private void Awake()
    {
        enemies = new NativeList<EntityData>(InitialAllocSize, Allocator.Persistent);

        enemy_objects = new List<Enemy>(InitialAllocSize);
        enemy_pool = new List<Enemy>(InitialAllocSize);
    }

    private void OnDestroy()
    {
        enemies.Dispose();
    }


    private void Start()
    {
    }


    void FixedUpdate()
    {
        if (!InitResources.GetPlayer) return;
        delta_time = Time.deltaTime;
        player_position = InitResources.GetPlayer.GetPosition();

        EntityMovementJob entity_job = new EntityMovementJob
        {
            enemies = enemies.AsDeferredJobArray(),
            delta_time = delta_time,
            player_position = player_position,

        };

        JobHandle handle = entity_job.Schedule(enemy_objects.Count, EntityJobBatchSize);

        handle.Complete();

        UpdateEntities();
    }




    [BurstCompile]
    struct EntityMovementJob : IJobParallelFor
    {
        public float delta_time;
        public float3 player_position;
        public NativeArray<EntityData> enemies;

        public void Execute(int index)
        {
            EntityData* ptr = &((EntityData*)enemies.GetUnsafePtr())[index];

            if (!ptr->active) return;
            ptr->direction = player_position - ptr->transform.position;
            ptr->velocity = ptr->direction * ptr->speed;

        }
    }



    public void TakeDmg(int ID, float dmg)
    {
        //Entity* ptr = GetEntity(index);
        EntityData* ptr = &((EntityData*)enemies.GetUnsafePtr())[ID];
        ptr->hp -= dmg;
        if (ptr->hp <= 0)
        {
            int final_index = enemy_objects.Count - 1;

            ptr->active = false;
            enemy_objects[ID].gameObject.SetActive(false);

            enemy_pool.Add(enemy_objects[ID]);

            enemy_objects[ID] = enemy_objects[final_index];
            enemy_objects.RemoveAt(final_index);
            enemy_objects[ID].ID = ID;

            *ptr = enemies[final_index];
            enemies.RemoveAt(final_index);
            ptr->ID = ID;
        }

    }

    public void SpawnEntity(TransformData src, float HP, float Speed, float DMG)
    {
        if (enemy_pool.Count == 0)
        {
            int newID = enemy_objects.Count;

            // Object
            Enemy e = Instantiate(enemy_ref);
            e.ID = newID;
            e.SetPosition(src.position);
            e.gameObject.SetActive(true);
            enemy_objects.Add(e);

            // Data
            enemies.Add(new EntityData
            {
                transform = src,
                direction = GetMoveDirection(InitResources.GetPlayer.GetPosition(), src.position),
                ID = newID,
                hp = HP,
                dmg = DMG,
                speed = Speed,
                active = true,
            });
        }
        else
        {
            Enemy e = enemy_pool[0];
            enemy_pool.RemoveAtSwapBack(0);

            int newID = enemy_objects.Count;

            e.ID = newID;
            enemy_objects.Add(e);

            enemies.Add(new EntityData
            {
                transform = src,
                direction = GetMoveDirection(InitResources.GetPlayer.GetPosition(), src.position),
                ID = newID,
                hp = HP,
                dmg = DMG,
                speed = Speed,
                active = true,
            });

            enemy_objects[newID].gameObject.SetActive(true);
        }
    }


    public void UpdateEntities()
    {

        for (int i = enemy_objects.Count - 1; i >= 0; i--)
        {
            EntityData* ptr = &((EntityData*)enemies.GetUnsafePtr())[i];
            //EntityData* ptr = GetEntityPtr(i);
            if (!ptr->active) continue;

            enemy_objects[i].SetVelocity(ptr->velocity);
        }
    }


    //HELPERS
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntityData* GetEntityPtr(int i) => &((EntityData*)enemies.GetUnsafePtr())[i];

    public float3 GetMoveDirection(in float3 target, in float3 src)
    {
        return math.normalizesafe(target - src);
    }
    public void SetMoveDirection(ref float3 output, in float3 target, in float3 src)
    {
        output = math.normalizesafe(target - src);
    }

    public void AddNewEntity(int ID)
    {
        // TODO
    }
}
