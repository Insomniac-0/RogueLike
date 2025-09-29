using System.Data.Common;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerData _player_template;
    [SerializeField] private WeaponData _weapon_template;
    private ProjectileManager _projectile_manager => InitResources.GetProjectileManager;



    float shoot_coldown;

    float3 mouse_pos;

    // TYPES

    struct PlayerStats
    {
        public float max_health;
        public float base_move_speed;
        public float attack_speed;
        public float ms_multiply;
        public float xp_multiply;
        public float pickup_range;
        public float dmg_multiply;
    }

    struct Weapon
    {
        public int count;
        public int projectile_hp;
        public float speed;
        public float fire_rate;
        public float base_dmg;
    }
    struct PlayerMoveData
    {
        public float3 position;
        public float3 direction;
        public float3 velocity;
        public float speed;
    }



    PlayerStats player_stats;
    PlayerMoveData player_data;
    Weapon weapon;

    void Awake()
    {

        InitializeStats();
        InitializeWeapon();



        player_data.speed = player_stats.base_move_speed * player_stats.ms_multiply;

    }
    void Start()
    {
        player_data.position = InitResources.GetPlayer.GetPosition();
    }
    void Update()
    {

        shoot_coldown -= Time.deltaTime;

        player_data.position = InitResources.GetPlayer.GetPosition();
        player_data.direction = math.normalizesafe(InitResources.GetInputReader.GetMoveDirection());

        if (InitResources.GetInputReader.is_shooting && shoot_coldown <= 0f)
        {
            TransformData data;
            data.position = player_data.position;
            mouse_pos = InitResources.GetInputReader.GetMousePositionWS();
            float2 direction = math.normalizesafe(mouse_pos.xy - data.position.xy);
            float angle_rad = math.atan2(direction.y, direction.x);
            float angle = math.degrees(angle_rad);
            if (angle < 0) angle += 360;
            data.rotation = quaternion.EulerXYZ(0f, 0f, angle);
            data.scale = new(1f, 1f, 1f);

            InitResources.GetProjectileManager.SpawnProjectile(data, new(direction.xy, 0f), weapon.projectile_hp, weapon.speed, weapon.base_dmg);
            shoot_coldown = 1f / weapon.fire_rate;
        }



        //Debug.Log($"Moving in direction: {player_data.direction}");

        player_data.velocity = player_data.speed * player_data.direction;
        InitResources.GetPlayer.SetVelocity(player_data.velocity);
    }


    void InitializeStats()
    {
        player_stats.base_move_speed = _player_template.BaseMovementSpeed;

        player_stats.max_health = _player_template.MaxHealth;
        player_stats.attack_speed = _player_template.AttackSpeed;
        player_stats.ms_multiply = _player_template.MS_Multiply;
        player_stats.xp_multiply = _player_template.XP_Multiply;
        player_stats.pickup_range = _player_template.PickUpRange;

    }

    void InitializeWeapon()
    {
        weapon.count = _weapon_template.ProjectileCount;
        weapon.projectile_hp = _weapon_template.BaseProjectileHP;
        weapon.speed = _weapon_template.BaseProjectileSpeed;
        weapon.fire_rate = _weapon_template.FireRate;
        weapon.base_dmg = _weapon_template.BaseDMG;
    }


}
