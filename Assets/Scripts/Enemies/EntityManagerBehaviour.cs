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
    public NativeList<EntityData> enemies;

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

        UpdateSetEntities();
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
            ptr->direction = math.normalizesafe(player_position - ptr->transform.position);
            ptr->velocity = ptr->direction * ptr->speed;

        }
    }



    public void TakeDmg(int ID, float dmg)
    {
        //Entity* ptr = GetEntity(index);
        EntityData* ptr = &((EntityData*)enemies.GetUnsafePtr())[ID];
        ptr->hp -= dmg;
    }

    public void SpawnEntity(TransformData src, float HP, float Speed, float DMG)
    {
        int newID = enemy_objects.Count;

        if (enemy_pool.Count == 0)
        {


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


            e.ID = newID;
            e.ResetMaterial();
            e.SetPosition(src.position);
            e.gameObject.SetActive(true);
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

        }
    }

    public void UpdateGetEntities()
    {
        for (int i = 0; i < enemy_objects.Count; i++)
        {
            EntityData* ptr = &((EntityData*)enemies.GetUnsafePtr())[i];
            //EntityData* ptr = GetEntityPtr(i);
            if (!ptr->active) continue;
            ptr->transform.position = enemy_objects[i].GetPosition();
        }
    }
    public void UpdateSetEntities()
    {

        for (int i = 0; i < enemy_objects.Count; i++)
        {
            EntityData* ptr = &((EntityData*)enemies.GetUnsafePtr())[i];
            EntityData e = enemies[i];
            //EntityData* ptr = GetEntityPtr(i);
            if (!ptr->active) continue;
            if (ptr->hp > 0)
            {
                enemy_objects[i].SetVelocity(ptr->velocity);
                ptr->transform.position = enemy_objects[i].GetPosition();
            }
            else
            {
                int last_index = enemy_objects.Count - 1;
                ptr->active = false;

                enemy_objects[i].gameObject.SetActive(false);
                enemy_pool.Add(enemy_objects[i]);

                enemy_objects[i] = enemy_objects[last_index];
                enemy_objects[last_index].gameObject.SetActive(false);
                enemy_objects.RemoveAt(last_index);
                if (last_index < i) enemy_objects[i].ID = i;

                *ptr = enemies[last_index];
                enemies.RemoveAt(last_index);
                ptr->ID = i;
                //if (last_index < i) enemies[i] = e;

            }
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

    public float GetDamage(int ID) => enemies[ID].dmg;

}
