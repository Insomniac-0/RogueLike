using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.VisualScripting;
using UnityEngine;

[BurstCompile]
public unsafe class EnemyManagerBehaviour : MonoBehaviour
{
    //[SerializeField] InputReader input;

    // REFS
    [SerializeField] Enemy enemy_ref;
    [SerializeField] EnemyDataSO[] EnemyDataTemplates;




    float delta_time;

    Int32 player_mask;

    const int InitialAllocSize = 100;
    const int EntityJobBatchSize = 16;
    // TYPES



    // ARRAYS
    public NativeList<EntityData> enemies;
    NativeArray<float3> line_positions;

    List<Enemy> enemy_objects;
    List<Enemy> enemy_pool;



    float3 mouse_pos;
    float3 player_position;

    private void OnDestroy()
    {
        enemies.Dispose();
    }

    public void Init()
    {
        enemies = new NativeList<EntityData>(InitialAllocSize, Allocator.Persistent);
        enemy_objects = new List<Enemy>(InitialAllocSize);
        enemy_pool = new List<Enemy>(InitialAllocSize);
    }

    public void CleanUp()
    {
        enemies.Dispose();
        enemy_objects.Clear();
        enemy_pool.Clear();
    }

    private void Start()
    {
        player_mask = InitResources.GetPlayerMask;
    }


    // void FixedUpdate()
    // {
    //     if (!InitResources.GetPlayer) return;
    //     delta_time = Time.deltaTime;
    //     player_position = InitResources.GetPlayer.GetPosition;


    //     EntityMovementJob entity_job = new EntityMovementJob
    //     {
    //         enemies = enemies.AsDeferredJobArray(),
    //         delta_time = delta_time,
    //         player_position = player_position,

    //     };

    //     JobHandle handle = entity_job.Schedule(enemy_objects.Count, EntityJobBatchSize);

    //     handle.Complete();

    //     UpdateEntities();
    //     for (int i = 0; i < enemy_objects.Count; i++)
    //     {
    //         EntityData* ptr = GetEntityPtr(i);
    //         if (!ptr->active) continue;

    //         ptr->rayhit = Physics2D.Raycast(ptr->transform.position.xy, ptr->direction.xy, ptr->attack_range, player_mask);
    //         enemy_objects[i].DrawRaycastLine(player_position);
    //         // if (ptr->rayhit.transform && ptr->rayhit.transform.TryGetComponent<Player>(out Player p))
    //         // {
    //         // }
    //     }
    // }

    public void FixedEnemyUpdate()
    {
        if (!InitResources.GetPlayer) return;
        delta_time = Time.deltaTime;
        player_position = InitResources.GetPlayer.GetPosition;


        EntityMovementJob entity_job = new EntityMovementJob
        {
            enemies = enemies.AsDeferredJobArray(),
            delta_time = delta_time,
            player_position = player_position,

        };

        JobHandle handle = entity_job.Schedule(enemy_objects.Count, EntityJobBatchSize);

        handle.Complete();

        UpdateEntities();
        // for (int i = 0; i < enemy_objects.Count; i++)
        // {
        //     EntityData* ptr = GetEntityPtr(i);
        //     if (!ptr->active) continue;

        //     ptr->rayhit = Physics2D.Raycast(ptr->transform.position.xy, ptr->direction.xy, ptr->attack_range, player_mask);
        //     enemy_objects[i].DrawRaycastLine(player_position);
        //     // if (ptr->rayhit.transform && ptr->rayhit.transform.TryGetComponent<Player>(out Player p))
        //     // {
        //     // }
        // }
    }

    [BurstCompile]
    struct EntityMovementJob : IJobParallelFor
    {
        public float delta_time;
        public float3 player_position;
        public NativeArray<EntityData> enemies;




        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        float GetDistanceToPlayer(EntityData* ptr) => math.distance(ptr->transform.position, player_position);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void SetEntityVelocity(EntityData* ptr, float speed) => ptr->velocity = ptr->direction * speed;



        void SetEntityDirection(EntityData* ptr, float angle_offset)
        {
            float2 target_direction = math.normalizesafe(player_position.xy - ptr->transform.position.xy);
            float angle_direction = math.atan2(target_direction.y, target_direction.x) + math.radians(angle_offset);
            float2 kiting_direction = new float2(math.cos(angle_direction), math.sin(angle_direction));

            float correction_amount = 0f;
            if (GetDistanceToPlayer(ptr) < ptr->range)
            {
                correction_amount = (ptr->range / GetDistanceToPlayer(ptr)) - 1f;
            }

            float2 correction_direction = -target_direction * correction_amount;

            float2 accum = kiting_direction + correction_direction;
            ptr->direction.xy = accum;
            ptr->direction.z = 0f;
        }

        public void Execute(int index)
        {
            EntityData* ptr = &((EntityData*)enemies.GetUnsafePtr())[index];

            if (!ptr->active) return;


            ptr->state = GetDistanceToPlayer(ptr) < ptr->range ? EnemyState.ATTACKING : EnemyState.CHASING;

            switch (ptr->state)
            {
                case EnemyState.CHASING:
                    SetEntityDirection(ptr, 0f);
                    SetEntityVelocity(ptr, ptr->speed);
                    break;
                case EnemyState.ATTACKING:
                    SetEntityDirection(ptr, 90f);
                    SetEntityVelocity(ptr, ptr->crawl_speed);
                    break;
            }

        }
    }


    public int GetEnemyCount => enemy_objects.Count;

    public void TakeDmg(int ID, float dmg)
    {
        EntityData* ptr = GetEntityPtr(ID);
        //EntityData* ptr = &((EntityData*)enemies.GetUnsafePtr())[ID];
        ptr->HP -= dmg;
    }

    public void SpawnEntity(EnemyDataSO data, TransformData src)
    {
        int newID = enemy_objects.Count;
        if (enemy_pool.Count == 0)
        {
            // Object
            Enemy e = Instantiate(enemy_ref);
            e.ID = newID;
            e.SetPosition(src.position);
            e.DMG = data.BaseDMG;
            e.gameObject.SetActive(true);
            enemy_objects.Add(e);

            // Data
            enemies.Add(new EntityData(data, src, newID));
        }
        else
        {
            Enemy e = enemy_pool[0];
            enemy_pool.RemoveAtSwapBack(0);

            // Object
            e.ID = newID;
            e.SetPosition(src.position);
            e.DMG = data.BaseDMG;
            e.gameObject.SetActive(true);
            enemy_objects.Add(e);

            // Data
            enemies.Add(new EntityData(data, src, newID));
        }
    }

    public void PauseEnemies()
    {
        if (enemy_objects.Count <= 0) return;
        for (int i = 0; i < enemy_objects.Count; i++)
        {
            enemy_objects[i].SetVelocity(float3.zero);
        }
    }
    public void UpdateEntities()
    {

        for (int i = enemy_objects.Count - 1; i >= 0; i--)
        {
            //EntityData* ptr = &((EntityData*)enemies.GetUnsafePtr())[i];
            EntityData* ptr = GetEntityPtr(i);
            EntityData e = enemies[i];
            if (!ptr->active) continue;
            if (ptr->HP > 0)
            {
                enemy_objects[i].SetVelocity(ptr->velocity);
                ptr->transform.position = enemy_objects[i].GetPosition();
                enemy_objects[i].UpdateEnemy();
            }
            else
            {
                int last_index = enemy_objects.Count - 1;
                ptr->active = false;

                GameObject particle = Instantiate(GraphicsResources.GetDeathParticles);
                particle.transform.position = enemy_objects[i].GetPosition();
                particle.SetActive(true);

                enemy_pool.Add(enemy_objects[i]);
                enemy_objects[i].gameObject.SetActive(false);



                enemy_objects.RemoveAt(last_index);
                if (last_index < i) enemy_objects[i].ID = i;

                *ptr = enemies[last_index];
                enemies.RemoveAt(last_index);
                ptr->ID = i;

                InitResources.GetUpgradeSystem.AddExperience(5f);
                InitResources.GetUpgradeSystem.AddScore(1);
                InitResources.GetEventChannel.TriggerScoreChange();
                //if (last_index < i) enemies[i] = e;
            }
        }
    }

    //HELPERS
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntityData* GetEntityPtr(int i) => &((EntityData*)enemies.GetUnsafePtr())[i];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float3 GetMoveDirection(in float3 target, in float3 src) => math.normalizesafe(target - src);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetMoveDirection(ref float3 output, in float3 target, in float3 src) => output = math.normalizesafe(target - src);


    void RemoveEnemy(int current, int last, EntityData* ptr, EntityData e)
    {

        ptr->active = false;
        // GameObject particle = Instantiate(GraphicsResources.GetDeathParticles);
        // particle.transform.position = enemy_objects[i].GetPosition();
        // particle.SetActive(true);

        enemy_objects[current].gameObject.SetActive(false);
        enemy_pool.Add(enemy_objects[current]);

        enemy_objects[current] = enemy_objects[last];
        enemy_objects[last].gameObject.SetActive(false);
        enemy_objects.RemoveAt(last);
        if (last < current) enemy_objects[current].ID = current;

        *ptr = enemies[last];
        enemies.RemoveAt(last);
        ptr->ID = current;
        //if (last < current) enemies[current] = e;
    }
}
