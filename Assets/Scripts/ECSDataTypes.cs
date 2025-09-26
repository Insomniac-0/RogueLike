using Unity.Mathematics;
using UnityEngine;


public struct TransformData
{
    public quaternion rotation;
    public float3 position;
    public float3 scale;

    public TransformData(in Transform t)
    {
        rotation = t.rotation;
        position = t.position;
        scale = t.localScale;
    }
}

public struct ProjectileData
{
    public TransformData transform;

    public float3 direction;

    public int ID;
    public int HP;

    public float dmg;
    public float speed;
    public float lifetime;

    public bool active;
}

public struct EntityData
{
    public TransformData transform;

    public float3 direction;
    public float3 velocity;

    public int ID;

    public float dmg;
    public float speed;
    public float hp;

    public bool active;

}