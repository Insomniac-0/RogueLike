using Unity.Mathematics;
using UnityEngine;


public enum ScalingType
{
    ADDITIVE,
    MULTIPLICATIVE,
}

public enum EnemyState
{
    CHASING,
    KITING,
    ATTACKING,
}

public enum EnemyType
{
    SKULL,
    BAT,
}

public struct Upgrade
{
    ScalingType scaling;
    float value;
}

public struct PlayerStats
{
    public float max_hp;
    public float current_hp;
    public float move_speed;
    public float attack_speed;
}

public struct PlayerBaseStats
{
    public float max_health;
    public float move_speed;
    public float attack_speed;
    public float pickup_range;
}

public struct PlayerMultipliers
{
    public float ms_multiply;
    public float xp_multiply;
    public float as_multiply;
    public float dmg_multiply;

    public PlayerMultipliers(float ms = 1.0f, float xp = 1.0f, float aspeed = 1.0f, float dmg = 1.0f)
    {
        ms_multiply = ms;
        xp_multiply = xp;
        as_multiply = aspeed;
        dmg_multiply = dmg;
    }
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
    public float speed;
    public float lifetime;
    public float dmg;

    public bool active;

    public ProjectileData(ProjectileDataSO data, TransformData t, float3 dir, int id, bool active = true)
    {
        transform = t;
        direction = dir;
        ID = id;
        HP = data.BaseHP;
        lifetime = data.Lifetime;
        speed = data.BaseSpeed;
        dmg = data.BaseDMG;
        this.active = active;
    }
}

public struct EntityData
{
    public RaycastHit2D rayhit;
    public TransformData transform;
    public EnemyState state;
    public EnemyType type;

    public float3 direction;
    public float3 velocity;
    public float3 aim_direction;


    public int ID;

    public float cooldown;
    public float counter;
    public float dmg;
    public float speed;
    public float crawl_speed;
    public float HP;
    public float attack_speed;
    public float attack_windup;
    public float range;
    public float attack_range;

    public bool active;

    public EntityData(EnemyDataSO data, TransformData t, int id, EnemyType type, bool active = true)
    {
        rayhit = new RaycastHit2D();
        state = EnemyState.CHASING;
        direction = float3.zero;
        velocity = float3.zero;
        aim_direction = float3.zero;
        this.type = type;

        transform = t;
        ID = id;

        cooldown = 0f;
        counter = 0f;
        HP = data.MaxHealth;
        speed = data.BaseMovementSpeed;
        crawl_speed = data.CrawlSpeed;
        dmg = data.BaseDMG;
        attack_speed = data.AttackSpeed;
        attack_windup = data.AttackWindup;
        range = data.Range;
        attack_range = data.AttackRange;

        this.active = active;

    }

    public enum ColliderType
    {
        ENTITY,
        PROJECTILE,
        ATTACK,
    }
}