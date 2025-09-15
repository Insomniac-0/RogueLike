using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Rendering;

public class EntityManagerBehaviour : MonoBehaviour
{
    [SerializeField] InputBehaviour input_behaviour;

    // REFS
    [SerializeField] Projectile bullet_ref;
    [SerializeField] Player player_ref;
    [SerializeField] Enemy enemy_ref;

    // FLOAT
    [SerializeField] float bullet_speed;
    [SerializeField] float enemy_speed;
    [SerializeField] float player_speed;

    // INT
    [SerializeField] int entity_count;
    [SerializeField] int bullet_count;


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
        public float speed;
        public bool active;
    }


    // ARRAYS
    Enemy[] enemy_objects;
    Projectile[] bullet_objects;

    NativeArray<Entity> entities;
    NativeArray<Entity> bullet_entities;

    NativeArray<float> delta_time;

    NativeArray<float3> player_position;


    Entity player_entity;
    Player player_object;
    float3 RED, GREEN, BLUE;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;

        player_object = Instantiate(player_ref);

        RED = new float3(1f, 0f, 0f);
        GREEN = new float3(0f, 1f, 0f);
        BLUE = new float3(0f, 0f, 1f);

        player_object.transform.position = new Vector3(0f, 0f, 0f);
        player_entity.position = player_object.transform.position;
        player_entity.direction = float3.zero;
        player_entity.speed = player_speed;
        player_entity.color = BLUE;

        entities = new NativeArray<Entity>(entity_count, Allocator.Persistent);
        bullet_entities = new NativeArray<Entity>(bullet_count, Allocator.Persistent);

        delta_time = new NativeArray<float>(1, Allocator.Persistent);
        player_position = new NativeArray<float3>(1, Allocator.Persistent);


        enemy_objects = new Enemy[entity_count];
        bullet_objects = new Projectile[bullet_count];

        for (int i = 0; i < enemy_objects.Length; i++)
        {
            enemy_objects[i] = Instantiate(enemy_ref);
            enemy_objects[i].transform.position = new Vector3(
                UnityEngine.Random.Range(-20f, 20f),
                UnityEngine.Random.Range(-20f, 20f),
                0);
            enemy_objects[i].gameObject.SetActive(true);
        }


        for (int i = 0; i < entity_count; i++)
        {
            Entity entity = entities[i];

            entity.position = enemy_objects[i].transform.position;
            entity.direction = float3.zero;
            entity.speed = enemy_speed;
            entity.color = RED;
            entity.active = true;

            entities[i] = entity;
        }
    }

    private void OnDestroy()
    {
        entities.Dispose();
        delta_time.Dispose();
        player_position.Dispose();
        bullet_entities.Dispose();
    }


    private void Start()
    {
        Application.targetFrameRate = 0;
        QualitySettings.vSyncCount = -1;
    }

    void Update()
    {


        delta_time[0] = Time.deltaTime;
        player_position[0] = player_object.GetPosition();





        EntityMovementJob entity_job = new EntityMovementJob
        {
            entities = entities,
            delta_time = delta_time,
            player_position = player_position,

        };


        JobHandle handle = entity_job.Schedule(entity_count, 16);

        handle.Complete();


        for (int i = 0; i < entity_count; i++)
        {
            if (!entities[i].active) continue;
            enemy_objects[i].SetPosition(entities[i].position);
            enemy_objects[i].SetColor(entities[i].color);

        }

        player_entity.direction.xy = input_behaviour.GetMoveDirection();
        player_entity.position += math.normalizesafe(player_entity.direction) * Time.deltaTime * player_entity.speed;
        player_object.SetPosition(player_entity.position);
        player_object.SetColor(player_entity.color);
        Camera.main.transform.position = player_object.GetPosition();
    }





    struct EntityMovementJob : IJobParallelFor
    {
        [NativeDisableParallelForRestriction]
        public NativeArray<float> delta_time;
        [NativeDisableParallelForRestriction]
        public NativeArray<float3> player_position;

        public NativeArray<Entity> entities;



        public void Execute(int index)
        {
            Entity entity = entities[index];

            if (!entity.active) return;

            entity.direction = math.normalizesafe(player_position[0] - entity.position);
            entity.position += entity.direction * entity.speed * delta_time[0];

            entities[index] = entity;
        }
    }

}
