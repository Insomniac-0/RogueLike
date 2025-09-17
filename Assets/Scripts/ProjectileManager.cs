using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using gthf;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Build.Pipeline.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public unsafe class ProjectileManager : MonoBehaviour
{
    [SerializeField] InputBehaviour input;
    [SerializeField] Projectile projectile_ref;

    [SerializeField] int projectile_count;

    float delta_time;



    // Unsafe.Read<Projectile>(ptr->obj_ptr).gameObject.SetActive(false);




    // TYPES
    struct S_Projectile
    {
        public float3 position;
        public float3 direction;
        public void* obj_ptr;
        public float speed;
        public bool active;
    }


    // ARRAYS
    NativeArray<S_Projectile> projectiles;


    private void Awake()
    {
        input.OnAbility += SpawnProjectile;
        projectiles = new NativeArray<S_Projectile>(projectile_count, Allocator.Persistent);

        for (int i = 0; i < projectile_count; i++)
        {
            Projectile p = Instantiate(projectile_ref);
            p.transform.position = Vector3.zero;
            p.gameObject.SetActive(false);


            S_Projectile* ptr = &((S_Projectile*)projectiles.GetUnsafePtr())[i];
            ptr->position = float3.zero;
            ptr->direction = float3.zero;
            ptr->speed = 0;
            ptr->active = false;
            ptr->obj_ptr = Unsafe.AsPointer(ref p);
        }
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
            projectiles = projectiles,
            delta_time = delta_time,
        };

        JobHandle handle = projectile_job.Schedule(projectile_count, 16);

        handle.Complete();

        for (int i = 0; i < projectile_count; i++)
        {
            S_Projectile* ptr = &((S_Projectile*)projectiles.GetUnsafePtr())[i];
            if (!ptr->active) continue;
            Unsafe.Read<Projectile>(ptr->obj_ptr).SetPosition(ptr->position);
        }
    }



    struct ProjectileMovementJob : IJobParallelFor
    {
        public NativeArray<S_Projectile> projectiles;
        public float delta_time;

        public void Execute(int index)
        {
            S_Projectile* ptr = &((S_Projectile*)projectiles.GetUnsafePtr())[index];
            if (!ptr->active) return;
            ptr->position += ptr->direction * ptr->speed * delta_time;

        }
    }

    public void SpawnProjectile()
    {
        Debug.Log("PEWPEW");
        for (int i = 0; i < projectile_count; i++)
        {
            if (!projectiles[i].active)
            {
                S_Projectile* ptr = &((S_Projectile*)projectiles.GetUnsafePtr())[i];
                ptr->speed = 20;
                ptr->position = float3.zero;
                ptr->direction = math.normalizesafe(input.GetMousePositionWS() - ptr->position);
                ptr->active = true;
                Unsafe.Read<Projectile>(ptr->obj_ptr).gameObject.SetActive(true);
            }
        }
        Debug.Log("No Inactive projectiles");
    }
}


