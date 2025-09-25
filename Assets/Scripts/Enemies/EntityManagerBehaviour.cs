using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Build.Pipeline.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Splines;

[BurstCompile]
public unsafe class EntityManagerBehaviour : MonoBehaviour
{
    //[SerializeField] InputReader input;
    Inputs inputs;

    // REFS
    [SerializeField] Enemy enemy_ref;

    // FLOAT
    [SerializeField] float enemy_speed;

    // INT
    [SerializeField] int entity_count;


    // TYPES

    public struct Entity
    {
        public float3 color;
        public float3 direction;
        public float3 position;
        public float3 velocity;
        public float speed;
        public float hp;
        public int ID;
        public bool active;

    }

    // ARRAYS
    Enemy[] enemy_objects;
    NativeArray<Entity> entities;


    float delta_time;


    // QUEUES
    NativeQueue<int> enemy_pool;


    float3 RED, GREEN, BLUE;

    float3 mouse_pos;

    private void Awake()
    {
        inputs = new Inputs();
        inputs.Enable();
        inputs.PlayerActions.Ability.performed += _ => SpawnEntity();
        //InitResources.Instance.OnAbility += SpawnEntity;
        //InitResources.Instance.InputHandler.PlayerActions.Ability.performed


        RED = new float3(1f, 0f, 0f);
        GREEN = new float3(0f, 1f, 0f);
        BLUE = new float3(0f, 0f, 1f);

        entities = new NativeArray<Entity>(entity_count, Allocator.Persistent);
        enemy_pool = new NativeQueue<int>(Allocator.Persistent);
        enemy_objects = new Enemy[entity_count];

        for (int i = 0; i < enemy_objects.Length; i++)
        {
            enemy_objects[i] = Instantiate(enemy_ref);
            enemy_objects[i].transform.position = float3.zero;
            enemy_objects[i].gameObject.SetActive(false);
            enemy_objects[i].ID = i;

            enemy_pool.Enqueue(i);

            Entity* ptr = &((Entity*)entities.GetUnsafePtr())[i];

            ptr->position = enemy_objects[i].transform.position;
            ptr->direction = float3.zero;
            ptr->speed = enemy_speed;
            ptr->active = false;
            ptr->ID = i;

        }

    }

    private void OnDestroy()
    {
        entities.Dispose();
        enemy_pool.Dispose();
        inputs.Dispose();
    }


    private void Start()
    {
        Application.targetFrameRate = 0;
        QualitySettings.vSyncCount = -1;
    }


    void FixedUpdate()
    {
        if (!InitResources.GetPlayer) return;
        delta_time = Time.deltaTime;

        EntityMovementJob entity_job = new EntityMovementJob
        {
            entities = entities,
            delta_time = delta_time,
            player_position = InitResources.GetNullableObjects.player.GetPosition(),

        };

        JobHandle handle = entity_job.Schedule(entity_count, 16);

        handle.Complete();

        for (int i = 0; i < entity_count; i++)
        {
            Entity* ptr = &((Entity*)entities.GetUnsafePtr())[i];
            if (!ptr->active) continue;

            if (enemy_objects[i].Health < 0)
            {
                ptr->active = false;
                enemy_objects[i].gameObject.SetActive(false);
                enemy_pool.Enqueue(i);
            }
            else
            {
                enemy_objects[i].SetVelocity(ptr->velocity);
                ptr->position = enemy_objects[i].GetPosition();
            }
        }
    }




    [BurstCompile]
    struct EntityMovementJob : IJobParallelFor
    {
        public float delta_time;
        public float3 player_position;
        public NativeArray<Entity> entities;

        public void Execute(int index)
        {
            Entity* ptr = &((Entity*)entities.GetUnsafePtr())[index];

            if (!ptr->active) return;
            ptr->direction = math.normalizesafe(player_position - ptr->position);
            ptr->velocity = ptr->direction * ptr->speed;

        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity* GetEntity(int i) => &((Entity*)entities.GetUnsafePtr())[i];

    public void TakeDmg(int index, float dmg)
    {
        //Entity* ptr = GetEntity(index);
        Entity* ptr = &((Entity*)entities.GetUnsafePtr())[index];
        ptr->hp -= dmg;
        if (ptr->hp <= 0) enemy_objects[index].gameObject.SetActive(false);
    }
    public void SpawnEntity()
    {
        int index = enemy_pool.Dequeue();

        Entity* ptr = &((Entity*)entities.GetUnsafePtr())[index];

        // Object
        enemy_objects[index].SetPosition(float3.zero);
        enemy_objects[index].gameObject.SetActive(true);

        // Data
        ptr->position = float3.zero;
        ptr->hp = 10;
        ptr->active = true;
    }

}
