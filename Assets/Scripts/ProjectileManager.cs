using System.Collections.Generic;
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



    // TYPES



    // ARRAYS

    NativeList<ProjectileData> projectiles;

    List<Projectile> p_objects;
    List<Projectile> p_ready_objects;


    private void Awake()
    {
        projectiles = new NativeList<ProjectileData>(InitialAllocSize, Allocator.Persistent);

        p_objects = new List<Projectile>(InitialAllocSize);
        p_ready_objects = new List<Projectile>(InitialAllocSize);
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

        JobHandle handle = projectile_job.Schedule(p_objects.Count, ProjectileJobBatchSize);

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
            p_objects[ID].gameObject.SetActive(false);
            ptr->active = false;
        }
    }
    public void SpawnProjectile(TransformData src, float3 target, int HP, float Speed, float DMG)
    {

        if (p_ready_objects.Count == 0)
        {
            int newID = p_objects.Count;
            // Objects
            Projectile p = Instantiate(projectile_ref);
            p.ID = newID;
            p.SetPosition(float3.zero);
            p.gameObject.SetActive(true);
            p_objects.Add(p);

            // Data


            projectiles.Add(new ProjectileData());
            ProjectileData* ptr = &((ProjectileData*)projectiles.GetUnsafePtr())[newID];

            ptr->ID = newID;
            ptr->HP = HP;
            ptr->lifetime = 2f;
            ptr->transform = src;
            ptr->direction = math.normalizesafe(target - src.position);
            ptr->speed = Speed;
            ptr->dmg = DMG;
            ptr->active = true;

        }
        else
        {
            Projectile p = p_ready_objects[0];
            p_ready_objects.RemoveAtSwapBack(0);
            int newID = p_objects.Count;
            p.ID = newID;
            float angle_rad = math.atan2(projectiles[newID].direction.y, projectiles[newID].direction.x);
            src.rotation = quaternion.EulerXYZ(0f, 0f, math.degrees(angle_rad));
            p_objects.Add(p);
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

            

            p_objects[newID].SetRotation(transform.rotation);
            p_objects[newID].gameObject.SetActive(true);
        }
    }
    void UpdateProjectiles()
    {
        for (int i = p_objects.Count - 1; i >= 0; i--)
        {
            ProjectileData* ptr = &((ProjectileData*)projectiles.GetUnsafePtr())[i];
            ProjectileData p = projectiles[i];
            if (!ptr->active) continue;

            if (ptr->lifetime > 0f && ptr->HP > 0)
            {
                p_objects[i].SetPosition(ptr->transform.position);
                ptr->lifetime -= delta_time;
            }
            else
            {
                int count = p_objects.Count;
                int final_index = count - 1;
                p.active = false;
                p_objects[i].gameObject.SetActive(false);
                p_ready_objects.Add(p_objects[i]);

                p_objects[i] = p_objects[final_index];
                p_objects.RemoveAt(final_index);
                if (final_index < i) p_objects[i].ID = i;

                projectiles[i] = projectiles[final_index];
                projectiles.RemoveAt(final_index);
                p.ID = i;
                if (final_index < i) projectiles[i] = p;
            }

        }
    }
}


