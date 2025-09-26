using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public unsafe class ProjectileManager : MonoBehaviour
{

    [SerializeField] Projectile projectile_ref;
    Player player;

    float delta_time;

    float3 mouse_pos;

    const int InitialAllocSize = 100;
    const int ProjectileJobBatchSize = 16;

    // ARRAYS

    NativeList<ProjectileData> projectiles;

    List<Projectile> projectile_objects;
    List<Projectile> projectile_pool;


    private void Awake()
    {
        projectiles = new NativeList<ProjectileData>(InitialAllocSize, Allocator.Persistent);

        projectile_objects = new List<Projectile>(InitialAllocSize);
        projectile_pool = new List<Projectile>(InitialAllocSize);
    }

    void Start()
    {
        mouse_pos = InitResources.GetInputReader.GetMousePositionWS();
    }

    private void OnDestroy()
    {
        projectiles.Dispose();
    }

    void Update()
    {
        delta_time = Time.deltaTime;

        ProjectileMovementJob projectile_job = new ProjectileMovementJob
        {
            projectiles = projectiles.AsDeferredJobArray(),
            delta_time = delta_time,
        };

        JobHandle handle = projectile_job.Schedule(projectile_objects.Count, ProjectileJobBatchSize);

        handle.Complete();

        UpdateProjectiles();
    }

    [BurstCompile]
    struct ProjectileMovementJob : IJobParallelFor
    {
        public NativeArray<ProjectileData> projectiles;
        public float delta_time;

        public void Execute(int index)
        {
            ProjectileData* ptr = &((ProjectileData*)projectiles.GetUnsafePtr())[index];
            if (!ptr->active) return;
            ptr->transform.position += ptr->direction * ptr->speed * delta_time;
        }
    }

    public void TakeDMG(int ID)
    {
        ProjectileData* ptr = &((ProjectileData*)projectiles.GetUnsafePtr())[ID];
        ptr->HP--;
        if (ptr->HP <= 0)
        {
            int final_index = projectile_objects.Count - 1;

            ptr->active = false;
            projectile_objects[ID].gameObject.SetActive(false);

            projectile_pool.Add(projectile_objects[ID]);

            projectile_objects[ID] = projectile_objects[final_index];
            projectile_objects.RemoveAt(final_index);
            projectile_objects[ID].ID = ID;

            *ptr = projectiles[final_index];
            projectiles.RemoveAt(final_index);
            ptr->ID = ID;
        }
    }

    public void SpawnProjectile(TransformData src, float3 target, int HP, float Speed, float DMG)
    {

        if (projectile_pool.Count == 0)
        {
            int newID = projectile_objects.Count;

            // Objects
            Projectile p = Instantiate(projectile_ref);
            p.ID = newID;
            p.SetPosition(src.position);
            p.gameObject.SetActive(true);
            projectile_objects.Add(p);

            // Data

            projectiles.Add(new ProjectileData
            {
                transform = src,
                direction = math.normalizesafe(target - src.position),
                ID = newID,
                HP = HP,
                dmg = DMG,
                speed = Speed,
                lifetime = 2f,
                active = true,
            });
        }
        else
        {
            Projectile p = projectile_pool[0];
            projectile_pool.RemoveAtSwapBack(0);

            int newID = projectile_objects.Count;
            p.ID = newID;


            projectile_objects.Add(p);
            projectiles.Add(new ProjectileData
            {
                speed = Speed,
                HP = HP,
                dmg = DMG,
                lifetime = 2f,
                transform = src,
                direction = math.normalizesafe(target - src.position),
                active = true,
                ID = newID,
            });


            float angle_rad = math.atan2(projectiles[newID].direction.y, projectiles[newID].direction.x);
            quaternion q = quaternion.EulerXYZ(0f, 0f, math.degrees(angle_rad));

            projectile_objects[newID].SetRotation(q);
            projectile_objects[newID].gameObject.SetActive(true);
        }
    }
    void UpdateProjectiles()
    {
        for (int i = projectile_objects.Count - 1; i >= 0; i--)
        {
            //ProjectileData* ptr = &((ProjectileData*)projectiles.GetUnsafePtr())[i];
            ProjectileData* ptr = GetProjectilePtr(i);
            ProjectileData p = projectiles[i];
            if (!ptr->active) continue;

            if (ptr->lifetime > 0f && ptr->HP > 0)
            {
                projectile_objects[i].SetPosition(ptr->transform.position);
                ptr->lifetime -= delta_time;
            }
            else
            {
                int count = projectile_objects.Count;
                int final_index = count - 1;
                ptr->active = false;

                projectile_objects[i].gameObject.SetActive(false);
                projectile_pool.Add(projectile_objects[i]);

                projectile_objects[i] = projectile_objects[final_index];
                projectile_objects.RemoveAt(final_index);
                if (final_index < i) projectile_objects[i].ID = i;

                //projectiles[i] = projectiles[final_index];
                *ptr = projectiles[final_index];
                projectiles.RemoveAt(final_index);
                ptr->ID = i;
                if (final_index < i) projectiles[i] = p;
            }
        }
    }

    void KillProjectile(int index, int end)
    {
        // TODO
    }

    // HELPERS
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ProjectileData* GetProjectilePtr(int i) => &((ProjectileData*)projectiles.GetUnsafePtr())[i];
}


