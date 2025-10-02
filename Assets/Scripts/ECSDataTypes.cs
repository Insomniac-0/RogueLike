using Unity.Mathematics;
using UnityEngine;


public enum ScalingType
{
    ADDITIVE,
    MULTIPLICATIVE,
}

public struct Upgrade
{
    ScalingType scaling;
}

public struct PlayerGameData
{
}

public struct Stats
{
    public float max_health;
    public float current_hp;
    public int lvl;
}

public struct MoveData
{
    public float3 position;
    public float3 direction;
    public float3 velocity;

    public float movement_speed;
}

public struct TransformData
{
    public quaternion rotation;
    public float3 position;
    public float3 scale;

    public TransformData(in Transform t)
    {
        position = t.position;
        rotation = t.rotation;
        scale = t.localScale;
    }
    public TransformData(in float3 position)
    {
        this.position = position;
        rotation = quaternion.EulerXYZ(0f, 0f, 0f);
        scale = new(1f, 1f, 1f);
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

public enum ColliderType
{
    ENTITY,
    PROJECTILE,
    ATTACK,
}
