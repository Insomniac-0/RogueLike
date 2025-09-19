using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Build.Pipeline.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public unsafe class ProjectileManager : MonoBehaviour
{
    [SerializeField] InputBehaviour input;
    [SerializeField] Projectile projectile_ref;
    [SerializeField] Player player;

    [SerializeField] int projectile_count;

    float delta_time;

    float shoot_coldown;

    float fire_rate;

    float3 mouse_pos;


    // Unsafe.Read<Projectile>(ptr->obj_ptr).gameObject.SetActive(false);




    // TYPES
    struct ProjectileData
    {
        public float3 position;
        public float3 direction;
        public float speed;
        public float lifetime;
        public bool active;
    }


    // ARRAYS
    NativeArray<ProjectileData> projectiles;

    Projectile[] p_objects;


    private void Awake()
    {
        shoot_coldown = 0f;
        fire_rate = 3f;
        projectiles = new NativeArray<ProjectileData>(projectile_count, Allocator.Persistent);
        p_objects = new Projectile[projectile_count];

        for (int i = 0; i < projectile_count; i++)
        {
            // Objects
            Projectile p = Instantiate(projectile_ref);
            p.transform.position = Vector3.zero;
            p.gameObject.SetActive(false);
            p_objects[i] = p;

            // Data
            ProjectileData* ptr = &((ProjectileData*)projectiles.GetUnsafePtr())[i];
            ptr->position = float3.zero;
            ptr->direction = float3.zero;
            ptr->speed = 0;
            ptr->active = false;
        }
    }


    private void OnDestroy()
    {
        projectiles.Dispose();
    }

    void Update()
    {
        mouse_pos = input.GetMousePositionWS();
        mouse_pos.z = 0;
        delta_time = Time.deltaTime;
        shoot_coldown -= delta_time;


        if (input.is_shooting && shoot_coldown <= 0f)
        {
            SpawnProjectile(player.GetPosition(), mouse_pos);
            shoot_coldown = 1f / fire_rate;
        }

        ProjectileMovementJob projectile_job = new ProjectileMovementJob
        {
            projectiles = projectiles,
            delta_time = delta_time,
        };

        JobHandle handle = projectile_job.Schedule(projectile_count, 16);

        handle.Complete();

        for (int i = 0; i < projectile_count; i++)
        {
            ProjectileData* ptr = &((ProjectileData*)projectiles.GetUnsafePtr())[i];
            if (!ptr->active) continue;

            if (ptr->lifetime > 0f && p_objects[i].hits > 0)
            {
                p_objects[i].SetPosition(ptr->position);
                ptr->lifetime -= delta_time;


            }
            else
            {
                ptr->active = false;
                p_objects[i].gameObject.SetActive(false);
            }

        }

    }



    struct ProjectileMovementJob : IJobParallelFor
    {
        public NativeArray<ProjectileData> projectiles;
        public float delta_time;

        public void Execute(int index)
        {
            ProjectileData* ptr = &((ProjectileData*)projectiles.GetUnsafePtr())[index];
            if (!ptr->active) return;
            ptr->position += ptr->direction * ptr->speed * delta_time;
        }
    }

    public void SpawnProjectile(float3 src, float3 target)
    {
        for (int i = 0; i < projectile_count; i++)
        {
            if (!projectiles[i].active)
            {
                ProjectileData* ptr = &((ProjectileData*)projectiles.GetUnsafePtr())[i];
                ptr->speed = 20;
                ptr->lifetime = 2f;
                ptr->position = src;
                ptr->direction = math.normalizesafe(target - src);
                ptr->active = true;
                p_objects[i].gameObject.SetActive(true);
                p_objects[i].hits = 1;
                p_objects[i].Collided = false;

                return;
            }
        }
    }
}


