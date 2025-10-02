using System;
using System.Data.Common;
using Unity.Mathematics;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerData _player_template;
    [SerializeField] private WeaponData _weapon_template;

    private ProjectileManager _projectile_manager => InitResources.GetProjectileManager;

    public event Action OnHealthChange;


    float shoot_cooldown;
    float iframe_cooldown;

    float3 mouse_pos;

    // TYPES

    public struct PlayerStats
    {
        public float max_health;
        public float current_hp;
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
    public struct PlayerMoveData
    {
        public float3 position;
        public float3 direction;
        public float3 velocity;
        public float speed;
    }



    public PlayerStats player_stats;
    public PlayerMoveData player_data;
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
        shoot_cooldown = 0;
        iframe_cooldown = 0;
    }
    void Update()
    {

        if (shoot_cooldown > 0) shoot_cooldown -= Time.deltaTime * weapon.fire_rate;
        if (iframe_cooldown > 0) iframe_cooldown -= Time.deltaTime;
        if (player_stats.current_hp <= 0) Debug.Log($"HP : {player_stats.current_hp} - DEATH");


        player_data.position = InitResources.GetPlayer.GetPosition();
        player_data.direction = math.normalizesafe(InitResources.GetInputReader.GetMoveDirection());

        if (InitResources.GetInputReader.is_shooting && shoot_cooldown <= 0f)
        {
            mouse_pos = InitResources.GetInputReader.GetMousePositionWS();
            TransformData data;
            data.position = player_data.position;
            float2 direction = math.normalizesafe(mouse_pos.xy - data.position.xy);
            data.rotation = quaternion.EulerXYZ(0f, 0f, 0f);
            data.scale = new(1f, 1f, 1f);

            InitResources.GetProjectileManager.SpawnProjectile(data, new(direction.xy, 0f), weapon.projectile_hp, weapon.speed, weapon.base_dmg * player_stats.dmg_multiply);
            shoot_cooldown = 1f;
        }



        //Debug.Log($"Moving in direction: {player_data.direction}");

        player_data.velocity = player_data.speed * player_data.direction;
        InitResources.GetPlayer.SetVelocity(player_data.velocity);
    }

    void InitializeStats()
    {
        player_stats.base_move_speed = _player_template.BaseMovementSpeed;

        player_stats.max_health = _player_template.MaxHealth;
        player_stats.current_hp = _player_template.MaxHealth;
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


    public void TakeDMG(float DMG)
    {
        player_stats.current_hp -= DMG;
        iframe_cooldown = 0.5f;
        Debug.Log($"{DMG} Taken : {player_stats.current_hp} HP left");
        OnHealthChange?.Invoke();
    }
}
