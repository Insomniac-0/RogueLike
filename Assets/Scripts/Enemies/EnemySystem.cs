using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;


public unsafe class EnemySystem : MonoBehaviour
{

    // TYPES
    struct EnemyData
    {
        public float3 position;
        public float3 direction;

        public float speed;
        public float health;
        public int id;
    }

    public struct CollisionEvent
    {
        public int sender_id;
        public int reciever_id;
    }

    private NativeArray<EnemyData> _enemy_data;

    public NativeList<CollisionEvent> collision_events;


    void Awake()
    {
        collision_events = new NativeList<CollisionEvent>(Allocator.Persistent);
    }

    void OnDestroy()
    {
        collision_events.Dispose();
    }

    void Update()
    {
        foreach (var collision in collision_events)
        {

            EnemyData* ptr = &((EnemyData*)_enemy_data.GetUnsafePtr())[collision.reciever_id];

        }
    }

    public void AddCollisionEvent(int senderID, int recieverID)
    {
        CollisionEvent e;
        e.sender_id = senderID;
        e.reciever_id = recieverID;
        collision_events.Add(e);
    }
}