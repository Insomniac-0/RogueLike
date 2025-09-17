using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
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
    [SerializeField] InputBehaviour input_behaviour;


    // REFS
    [SerializeField] Player player_ref;
    [SerializeField] Enemy enemy_ref;

    // FLOAT
    [SerializeField] float enemy_speed;
    [SerializeField] float player_speed;

    // INT
    [SerializeField] int entity_count;


    // TYPES
    enum EntityType
    {
        PLAYER,
        ENEMY,
        PROJECTILE,
    }

    struct Entity
    {
        public float3 color;
        public float3 direction;
        public float3 position;
        public float3 velocity;
        public float speed;
        public bool active;
    }

    // ARRAYS
    Enemy[] enemy_objects;

    NativeArray<Entity> entities;
    //NativeArray<Entity> bullet_entities;

    NativeArray<float> delta_time;
    NativeArray<float3> player_position;


    // QUEUES
    NativeQueue<int> enemy_pool;


    Entity player_entity;
    Player player_object;
    float3 RED, GREEN, BLUE;

    float3 mouse_pos;

    private void Awake()
    {
        input_behaviour.OnAbility += SpawnEntity;
        Span<int> asd = stackalloc int[5];
        Cursor.lockState = CursorLockMode.Confined;

        player_object = Instantiate(player_ref);

        RED = new float3(1f, 0f, 0f);
        GREEN = new float3(0f, 1f, 0f);
        BLUE = new float3(0f, 0f, 1f);

        NativeList<int> ints = new(Allocator.Persistent);
        ints.AsDeferredJobArray();

        //Entity* lmao = (Entity*)entities.GetUnsafePtr();

        player_object.transform.position = new Vector3(0f, 0f, 0f);
        player_entity.position = player_object.transform.position;
        player_entity.direction = float3.zero;
        player_entity.speed = player_speed;
        player_entity.color = BLUE;

        entities = new NativeArray<Entity>(entity_count, Allocator.Persistent);

        enemy_pool = new NativeQueue<int>(Allocator.Persistent);





        delta_time = new NativeArray<float>(1, Allocator.Persistent);
        player_position = new NativeArray<float3>(1, Allocator.Persistent);


        enemy_objects = new Enemy[entity_count];

        for (int i = 0; i < enemy_objects.Length; i++)
        {
            enemy_objects[i] = Instantiate(enemy_ref);
            enemy_objects[i].transform.position = float3.zero;
            enemy_objects[i].gameObject.SetActive(false);

            enemy_pool.Enqueue(i);
        }


        for (int i = 0; i < entity_count; i++)
        {
            Entity entity = entities[i];

            entity.position = enemy_objects[i].transform.position;
            entity.direction = float3.zero;
            entity.speed = enemy_speed;
            entity.color = RED;
            entity.active = false;

            entities[i] = entity;
        }
    }

    private void OnDestroy()
    {
        entities.Dispose();
        delta_time.Dispose();
        player_position.Dispose();
        enemy_pool.Dispose();
    }


    private void Start()
    {
        Application.targetFrameRate = 0;
        QualitySettings.vSyncCount = -1;
    }


    void FixedUpdate()
    {



        player_position[0] = player_object.GetPosition();
        mouse_pos = input_behaviour.GetMousePositionWS();
        mouse_pos.z = 0;
        delta_time[0] = Time.deltaTime;




        EntityMovementJob entity_job = new EntityMovementJob
        {
            entities = entities,
            delta_time = delta_time,
            player_position = player_position,

        };

        float3 a, b, c;
        a = float3.zero;
        b = 1f;
        c = 2f;
        ComputeFunnyNumber(ref a, ref b, ref c);

        JobHandle handle = entity_job.Schedule(entity_count, 16);

        handle.Complete();

        for (int i = 0; i < entity_count; i++)
        {
            Entity* ptr = &((Entity*)entities.GetUnsafePtr())[i];
            if (!ptr->active) continue;



            enemy_objects[i].SetVelocity(ptr->velocity);
            enemy_objects[i].SetColor(ptr->color);

            ptr->position = enemy_objects[i].GetPosition();

            // enemy_objects[i].SetVelocity(entities[i].velocity);
            // enemy_objects[i].SetColor(entities[i].color);

            // Entity entity = entities[i];

            // entity.position = enemy_objects[i].GetPosition();

            // entities[i] = entity;

        }

        player_entity.direction = input_behaviour.GetMoveDirection();
        player_entity.position += math.normalizesafe(player_entity.direction) * Time.deltaTime * player_entity.speed;
        player_object.SetPosition(player_entity.position);
        player_object.SetColor(player_entity.color);

    }





    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static void ComputeFunnyNumber(ref float3 output, ref float3 a, ref float3 b)
    {
        float3 intermediate = a / b;
        output *= intermediate;
    }
    [BurstCompile]
    struct EntityMovementJob : IJobParallelFor
    {
        [NativeDisableParallelForRestriction]
        public NativeArray<float> delta_time;
        [NativeDisableParallelForRestriction]
        public NativeArray<float3> player_position;

        public NativeArray<Entity> entities;



        public void Execute(int index)
        {
            Entity* ptr = &((Entity*)entities.GetUnsafePtr())[index];


            if (!ptr->active) return;
            ptr->direction = math.normalizesafe(player_position[0] - ptr->position);
            ptr->velocity = ptr->direction * ptr->speed;

            //Entity entity = entities[index];
            // entity.direction = math.normalizesafe(player_position[0] - entity.position);
            // entity.velocity = entity.direction * entity.speed;
            //entities[index] = entity;
        }
    }

    public void SpawnEntity()
    {
        int index = enemy_pool.Dequeue();


        Entity entity = entities[index];

        entity.position = float3.zero;
        enemy_objects[index].SetPosition(float3.zero);

        enemy_objects[index].gameObject.SetActive(true);
        entity.active = true;

        entities[index] = entity;
    }


}
