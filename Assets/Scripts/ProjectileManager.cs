using System;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] Projectile projectile_ref;
    Player player;
    [SerializeField] InputReader _input;


    float delta_time;

    float3 mouse_pos;

    const int InitialAllocSize = 100;

    // Unsafe.Read<Projectile>(ptr->obj_ptr).gameObject.SetActive(false);


    const int ProjectileJobBatchSize = 16;



    // TYPES
    struct ProjectileData
    {
        public float3 position;
        public float3 direction;

        public int ID;
        public int hp;

        public float dmg;
        public float speed;
        public float lifetime;


        public bool active;
    }


    // ARRAYS

    NativeList<ProjectileData> projectiles;

    List<Projectile> p_objects;
    List<Projectile> p_ready_objects;


    private void Awake()
    {
        mouse_pos = _input.GetMousePositionWS();
        projectiles = new NativeList<ProjectileData>(InitialAllocSize, Allocator.Persistent);

        p_objects = new List<Projectile>(InitialAllocSize);
        p_ready_objects = new List<Projectile>(InitialAllocSize);

        // for (int i = 0; i < projectile_count; i++)
        // {
        //     // Objects
        //     Projectile p = Instantiate(projectile_ref);
        //     p.ID = i;
        //     p.transform.position = Vector3.zero;
        //     p.gameObject.SetActive(false);
        //     p_objects[i] = p;

        //     // Data
        //     ProjectileData* ptr = &((ProjectileData*)projectiles.GetUnsafePtr())[i];
        //     ptr->ID = i;
        //     ptr->hp = 0;
        //     ptr->position = float3.zero;
        //     ptr->direction = float3.zero;
        //     ptr->speed = 0;
        //     ptr->active = false;
        // }
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

        JobHandle handle = projectile_job.Schedule(projectiles.Count(), ProjectileJobBatchSize);

        handle.Complete();

        for (int i = 0; i < projectiles.Count(); i++)
        {
            ProjectileData* ptr = &((ProjectileData*)projectiles.GetUnsafePtr())[i];
            if (!ptr->active) continue;

            if (ptr->lifetime > 0f && ptr->hp > 0)
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

    public void TakeDMG(int ID)
    {
        ProjectileData* ptr = &((ProjectileData*)projectiles.GetUnsafePtr())[ID];
        ptr->hp--;
        if (ptr->hp <= 0)
        {
            p_objects[ID].gameObject.SetActive(false);
            ptr->active = false;
        }
    }
    public void SpawnProjectile(float3 src, float3 target, int HP, float Speed, float DMG)
    {
        if (p_ready_objects.Count == 0)
        {

        }
        else
        {
            Projectile p = p_ready_objects[0];
            int newID = p_objects.Count;
            p.ID = newID;
            p_objects.Add(p);
            projectiles.Add(new ProjectileData
            {
                speed = Speed,
                hp = HP,
                dmg = DMG,
                lifetime = 2f,
                position = src,
                direction = math.normalizesafe(target - src),
                active = true,
                ID = newID,
            });

            float angle_rad = math.atan2(projectiles[newID].direction.y, projectiles[newID].direction.x);

            p_objects[newID].transform.rotation = Quaternion.Euler(new float3(0f, 0f, math.degrees(angle_rad)));
            p_objects[newID].gameObject.SetActive(true);
        }
        // for (int i = 0; i < ; i++)
        // {
        //     if (!projectiles[i].active)
        //     {
        //         ProjectileData* ptr = &((ProjectileData*)projectiles.GetUnsafePtr())[i];
        //         ptr->speed = Speed;
        //         ptr->hp = HP;
        //         ptr->dmg = DMG;
        //         ptr->lifetime = 2f;
        //         ptr->position = src;
        //         ptr->direction = math.normalizesafe(target - src);
        //         ptr->active = true;

        //         float angle_rad = math.atan2(ptr->direction.y, ptr->direction.x);


        //         p_objects[i].transform.rotation = Quaternion.Euler(new float3(0f, 0f, math.degrees(angle_rad)));
        //         p_objects[i].gameObject.SetActive(true);


        //         return;
        //     }
        // }
    }
}


