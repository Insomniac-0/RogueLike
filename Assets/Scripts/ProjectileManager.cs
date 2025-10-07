using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public unsafe class ProjectileManager : MonoBehaviour
{

    [SerializeField] Projectile projectile_ref;
    [SerializeField] private List<ProjectileDataSO> _projectile_templates;

    private bool _initialized;


    Player player;

    float delta_time;

    float3 mouse_pos;

    const int InitialAllocSize = 100;
    const int ProjectileJobBatchSize = 16;

    // ARRAYS

    NativeList<ProjectileData> projectiles;

    List<Projectile> projectile_objects;
    List<Projectile> projectile_pool;

    void Start()
    {
        mouse_pos = InitResources.GetInputReader.GetMousePositionWS();
    }


    public void Init()
    {
        projectiles = new NativeList<ProjectileData>(InitialAllocSize, Allocator.Persistent);
        projectile_objects = new List<Projectile>(InitialAllocSize);
        projectile_pool = new List<Projectile>(InitialAllocSize);
    }
    public void HardCleanUp()
    {
        projectiles.Dispose();
        projectile_objects.Clear();
        projectile_pool.Clear();
    }

    public void SoftCleanUp()
    {
        for (int i = 0; i < projectile_objects.Count; i++)
        {
            Destroy(projectile_objects[i].gameObject);
        }
        projectiles.Clear();
        projectile_objects.Clear();
        projectile_pool.Clear();
    }


    public void FixedProjectileUpdate()
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
    }

    public void SpawnProjectile(ProjectileDataSO data, TransformData src, float3 direction, float dmg_multiply = 1.0f)
    {

        int newID = projectile_objects.Count;

        if (projectile_pool.Count == 0)
        {


            // Objects
            Projectile p = Instantiate(projectile_ref);

            p.ID = newID;
            p.SetPosition(src.position);
            p.SetRotation(src.rotation);
            p.collided = false;
            p._prevID = -1;
            p.DMG = data.BaseDMG * dmg_multiply;
            p.gameObject.SetActive(true);
            projectile_objects.Add(p);

            projectiles.Add(new ProjectileData(data, src, direction, newID));
        }
        else
        {


            // Object

            Projectile p = projectile_pool[0];
            projectile_pool.RemoveAtSwapBack(0);


            p.ID = newID;
            p.SetPosition(src.position);
            p.SetRotation(src.rotation);
            p.collided = false;
            p._prevID = -1;
            p.DMG = data.BaseDMG * dmg_multiply;
            p.gameObject.SetActive(true);
            projectile_objects.Add(p);

            projectiles.Add(new ProjectileData(data, src, direction, newID));
        }
    }

    void UpdateProjectiles()
    {
        if (projectile_objects.Count <= 0) return;
        for (int i = projectile_objects.Count - 1; i >= 0; i--)
        {
            ProjectileData* ptr = GetProjectilePtr(i);
            ProjectileData p = projectiles[i];
            if (!ptr->active) continue;

            if (ptr->lifetime > 0f && ptr->HP > 0f)
            {
                projectile_objects[i].SetPosition(ptr->transform.position);
                ptr->lifetime -= delta_time;
            }
            else
            {
                int last_index = projectile_objects.Count - 1;
                ptr->active = false;

                projectile_objects[i].gameObject.SetActive(false);
                projectile_pool.Add(projectile_objects[i]);

                projectile_objects[i] = projectile_objects[last_index];
                projectile_objects.RemoveAt(last_index);
                if (last_index < i) projectile_objects[i].ID = i;


                *ptr = projectiles[last_index];
                ProjectileData pr = projectiles[last_index];
                pr.active = false;
                projectiles.RemoveAt(last_index);
                ptr->ID = i;
                //if (last_index < i) projectiles[i] = p;
            }
        }
    }

    // HELPERS
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ProjectileData* GetProjectilePtr(int i) => &((ProjectileData*)projectiles.GetUnsafePtr())[i];
}


