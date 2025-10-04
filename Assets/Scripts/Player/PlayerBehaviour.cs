using System;
using System.Data.Common;
using Unity.Mathematics;
using UnityEditor.SearchService;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerDataSO _player_template;
    [SerializeField] private ProjectileDataSO _projectile_template;
    [SerializeField] float FireRate;

    private ProjectileManager _projectile_manager => InitResources.GetProjectileManager;

    public event Action OnHealthChange;




    float shoot_cooldown;
    float iframe_cooldown;


    float3 mouse_pos;
    float3 direction;


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

    public struct PlayerMoveData
    {
        public float3 position;
        public float3 direction;
        public float3 velocity;
        public float speed;
    }



    public PlayerStats player_stats;
    public PlayerMoveData player_data;

    void Awake()
    {

        InitializeStats();



        player_data.speed = player_stats.base_move_speed * player_stats.ms_multiply;

    }
    void Start()
    {
        player_data.position = InitResources.GetPlayer.GetPosition;
        shoot_cooldown = 0;
        iframe_cooldown = 0;
        direction = float3.zero;
    }
    void Update()
    {

        if (shoot_cooldown > 0) shoot_cooldown -= Time.deltaTime * FireRate;
        if (iframe_cooldown > 0) iframe_cooldown -= Time.deltaTime;
        if (player_stats.current_hp <= 0) Debug.Log($"HP : {player_stats.current_hp} - DEATH");


        player_data.position = InitResources.GetPlayer.GetPosition;
        player_data.direction = math.normalizesafe(InitResources.GetInputReader.GetMoveDirection());

        if (InitResources.GetInputReader.is_shooting && shoot_cooldown <= 0f)
        {
            mouse_pos = InitResources.GetInputReader.GetMousePositionWS();
            TransformData data;
            data.position = player_data.position;
            direction.xy = math.normalizesafe(mouse_pos.xy - data.position.xy);
            direction.z = 0f;
            data.rotation = quaternion.EulerXYZ(0f, 0f, 0f);
            data.scale = new(1f, 1f, 1f);
            InitResources.GetProjectileManager.SpawnProjectile(_projectile_template, data, direction);
            shoot_cooldown = 1f;

            //InitResources.GetProjectileManager.SpawnProjectile()
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


    public void TakeDMG(float DMG)
    {
        player_stats.current_hp -= DMG;
        iframe_cooldown = 0.5f;
        Debug.Log($"{DMG} Taken : {player_stats.current_hp} HP left");
        OnHealthChange?.Invoke();
    }

    public float GetHealthPercentage => player_stats.current_hp / player_stats.max_health;

}
