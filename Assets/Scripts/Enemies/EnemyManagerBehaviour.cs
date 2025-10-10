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
using UnityEngine;

[BurstCompile]
public unsafe class EnemyManagerBehaviour : MonoBehaviour
{
    //[SerializeField] InputReader input;

    // REFS
    [SerializeField] Enemy enemy_ref;
    [SerializeField] Sprite[] sprites;
    [SerializeField] SoundData laser_sound;
    [SerializeField] SoundData BatDeathSound;
    [SerializeField] SoundData SkullDeathSound;

    float delta_time;


    Int32 player_mask;


    private bool disposed;

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

    void OnDestroy()
    {
        if (!disposed) enemies.Dispose();
    }


    public void Init()
    {
        enemies = new NativeList<EntityData>(InitialAllocSize, Allocator.Persistent);
        enemy_objects = new List<Enemy>(InitialAllocSize);
        enemy_pool = new List<Enemy>(InitialAllocSize);
        disposed = false;
    }

    public void HardCleanUp()
    {
        enemies.Dispose();
        enemy_objects.Clear();
        enemy_pool.Clear();
        disposed = true;

    }

    public void SoftCleanUp()
    {
        for (int i = 0; i < enemy_objects.Count; i++)
        {
            Destroy(enemy_objects[i].gameObject);
        }
        enemies.Clear();
        enemy_objects.Clear();
        enemy_pool.Clear();
    }

    private void Start()
    {
        player_mask = InitResources.GetPlayerMask;
    }



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
    }

    [BurstCompile]
    struct EntityMovementJob : IJobParallelFor
    {
        public float delta_time;
        public float3 player_position;
        public NativeArray<EntityData> enemies;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        EntityData* GetEntityPtr(int i) => &((EntityData*)enemies.GetUnsafePtr())[i];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        float GetDistanceToPlayer(EntityData* ptr) => math.distance(ptr->transform.position, player_position);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void SetEntityVelocity(EntityData* ptr, float speed) => ptr->velocity = ptr->direction * speed;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        float2 GetTargetDirection(EntityData* ptr) => math.normalizesafe(player_position.xy - ptr->transform.position.xy);

        void SetAimDirection(EntityData* ptr) => ptr->aim_direction.xy = GetTargetDirection(ptr);


        void SetEntityMoveDirection(EntityData* ptr, float angle_offset)
        {
            float2 target_direction = GetTargetDirection(ptr);
            if (angle_offset > 0f)
            {
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
            }
            else
            {
                ptr->direction.xy = target_direction.xy;

            }
            ptr->direction.z = 0f;
        }


        void UpdateState(EntityData* ptr)
        {
            bool in_range = GetDistanceToPlayer(ptr) < ptr->range ? true : false;
            if (!in_range && ptr->state != EnemyState.CHASING)
            {
                ptr->state = EnemyState.CHASING;
                return;
            }

            if (in_range && ptr->cooldown <= 0 && ptr->state != EnemyState.ATTACKING)
            {
                ptr->state = EnemyState.ATTACKING;
                SetAimDirection(ptr);
                ptr->aim_direction.z = 0;
                ptr->velocity = float3.zero;
                ptr->counter = ptr->attack_windup;
                return;
            }
            if (in_range && ptr->cooldown > 0 && ptr->state != EnemyState.KITING)
            {
                ptr->state = EnemyState.KITING;
                return;
            }

        }

        public void Execute(int index)
        {
            EntityData* ptr = GetEntityPtr(index);

            if (!ptr->active) return;
            if (ptr->cooldown > 0f) ptr->cooldown -= delta_time * ptr->attack_speed;
            UpdateState(ptr);

            switch (ptr->state)
            {
                case EnemyState.CHASING:
                    SetEntityMoveDirection(ptr, 0f);
                    SetEntityVelocity(ptr, ptr->speed);
                    SetAimDirection(ptr);
                    ptr->aim_direction.z = 0;
                    break;
                case EnemyState.KITING:
                    SetEntityMoveDirection(ptr, 90f);
                    SetEntityVelocity(ptr, ptr->crawl_speed);
                    SetAimDirection(ptr);
                    ptr->aim_direction.z = 0;
                    break;
                case EnemyState.ATTACKING:
                    ptr->counter -= delta_time;
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

    public void SpawnEntity(EnemyDataSO data, TransformData src, EnemyType type)
    {
        int newID = enemy_objects.Count;
        if (enemy_pool.Count == 0)
        {
            // Object
            Enemy e = Instantiate(enemy_ref);
            e.ID = newID;
            e.SetPosition(src.position);
            e.DMG = data.BaseDMG;
            e.blink_strength = 0;
            e.SetSprite(sprites[(int)type]);
            e.gameObject.SetActive(true);
            enemy_objects.Add(e);

            // Data
            enemies.Add(new EntityData(data, src, newID, type));
        }
        else
        {
            Enemy e = enemy_pool[0];
            enemy_pool.RemoveAtSwapBack(0);

            // Object
            e.ID = newID;
            e.SetPosition(src.position);
            e.DMG = data.BaseDMG;
            e.blink_strength = 0;
            e.SetSprite(sprites[(int)type]);
            e.gameObject.SetActive(true);
            enemy_objects.Add(e);

            // Data
            enemies.Add(new EntityData(data, src, newID, type));
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
            if (!ptr->active) continue;
            if (ptr->HP > 0)
            {
                enemy_objects[i].SetVelocity(ptr->velocity);
                ptr->transform.position = enemy_objects[i].GetPosition();
                if (ptr->type != EnemyType.BAT || ptr->state == EnemyState.CHASING) enemy_objects[i].ShowLineRenderer(false);

                if (ptr->state == EnemyState.ATTACKING && ptr->type == EnemyType.BAT)
                {
                    enemy_objects[i].ShowLineRenderer(true);
                    enemy_objects[i].DrawRaycastLine(ptr->transform.position, ptr->aim_direction, ptr->attack_range);
                    if (ptr->counter <= 0)
                    {
                        InitResources.GetSoundManager.SpawnSound(laser_sound);
                        ptr->cooldown = 1f;
                        ptr->counter = ptr->attack_windup;
                        enemy_objects[i].SetLineWidth(1f);
                        ptr->rayhit = Physics2D.Raycast(ptr->transform.position.xy, ptr->aim_direction.xy, ptr->attack_range, player_mask);
                        if (ptr->rayhit.transform && ptr->rayhit.transform.TryGetComponent<PlayerBehaviour>(out PlayerBehaviour p))
                        {
                            p.TakeDMG(ptr->dmg);
                        }
                    }
                }

                enemy_objects[i].UpdateEnemy();
            }
            else
            {
                int score;
                int xp;

                switch (ptr->type)
                {
                    case EnemyType.SKULL:
                        score = 1;
                        xp = 5;
                        InitResources.GetSoundManager.SpawnSound(SkullDeathSound);
                        break;
                    case EnemyType.BAT:
                        score = 2;
                        xp = 10;
                        InitResources.GetSoundManager.SpawnSound(BatDeathSound);
                        break;
                    default:
                        score = 0;
                        xp = 0;
                        break;
                }

                ptr->active = false;

                GameObject particle = Instantiate(GraphicsResources.GetDeathParticles);
                particle.transform.position = enemy_objects[i].GetPosition();
                particle.SetActive(true);

                enemy_objects[i].gameObject.SetActive(false);
                enemy_pool.Add(enemy_objects[i]);

                enemy_objects.RemoveAtSwapBack(i);
                enemies.RemoveAtSwapBack(i);
                int last_index = enemy_objects.Count - 1;

                if (last_index < i)
                {
                    enemy_objects[last_index].ID = last_index;
                    EntityData e = enemies[last_index];
                    e.ID = last_index;
                }
                else
                {
                    enemy_objects[i].ID = i;
                    EntityData e = enemies[i];
                    e.ID = i;
                }


                InitResources.GetUpgradeSystem.AddExperience(xp);
                InitResources.GetUpgradeSystem.AddScore(score);
                InitResources.GetUpgradeSystem.UpdateKills();
                InitResources.GetEventChannel.TriggerScoreChange();
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


}
