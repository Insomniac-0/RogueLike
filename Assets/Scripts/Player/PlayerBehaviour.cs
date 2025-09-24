using System;
using Unity.Mathematics;
using Unity.VisualScripting.FullSerializer;
using UnityEditorInternal;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private Float3Variable player_position;
    [SerializeField] private PlayerData _player_template;
    [SerializeField] private WeaponData _weapon_template;
    [SerializeField] private ProjectileManager _projectile_manager;

    [SerializeField] private Player _player;
    [SerializeField] private InputReader _input;


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
        _input.OnMove += GetMoveDir;
        InitializeStats();
        InitializeWeapon();

        player_data.position = _player.GetPosition();
        player_data.direction = _input.GetMoveDirection();
        player_data.speed = player_stats.base_move_speed * player_stats.ms_multiply;

        player_position.Value = player_data.position;
    }
    void Update()
    {
        mouse_pos.xy = _input.GetMousePositionWS().xy;
        mouse_pos.z = 0;
        shoot_coldown -= Time.deltaTime;

        if (_input.is_shooting && shoot_coldown <= 0f)
        {
            _projectile_manager.SpawnProjectile(_player.GetPosition(), mouse_pos, weapon.projectile_hp, weapon.speed, weapon.base_dmg);
            shoot_coldown = 1f / weapon.fire_rate;
        }

        player_data.direction = math.normalizesafe(_input.GetMoveDirection());

        Debug.Log($"Moving in direction: {player_data.direction}");



        player_data.velocity = player_data.speed * player_data.direction;
        _player.SetVelocity(player_data.velocity);
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

    void GetMoveDir()
    {
        player_data.direction = _input.GetMoveDirection();
    }
}
