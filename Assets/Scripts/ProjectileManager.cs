using gthf;
using Unity.Collections;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class ProjectileManager : MonoBehaviour
{
    [SerializeField] Projectile prefab;
    [SerializeField] int projectile_count;

    struct Projectile
    {
        public float3 direction;
        public float3 speed;
    }

    NativeArray<Projectile> projectiles;


    void Awake()
    {
        projectiles = new NativeArray<Projectile>(projectile_count, Allocator.Persistent);
    }

    void Update()
    {

    }


    // struct ProjectileMovementJob : IJobParallelFor
    // {

    //     [NativeDisableParallelForRestriction]
    //     public NativeArray<float> delta_time;

    //     public NativeArray<Entity> projectiles;

    //     public void Execute(int index)
    //     {
    //         Entity* ptr = &((Entity*)projectiles.GetUnsafePtr())[index];
    //         ptr->position += ptr->velocity;
    //     }
    // }

}