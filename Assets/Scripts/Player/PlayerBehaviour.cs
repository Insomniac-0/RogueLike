using Unity.Mathematics;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerDataSO _player_template;
    [SerializeField] private ProjectileDataSO _projectile_template;
    [SerializeField] float FireRate;

    private ProjectileManager _projectile_manager => InitResources.GetProjectileManager;

    float shoot_cooldown;
    float iframe_cooldown;


    float3 mouse_pos;
    float3 direction;


    // TYPES


    public struct PlayerMoveData
    {
        public float3 position;
        public float3 direction;
        public float3 velocity;
        public float speed;
    }

    PlayerMultipliers multipliers;
    public PlayerMoveData player_data;
    public PlayerStats player_stats;

    void Awake()
    {
        InitResources.GetGameManager.InitializePlayerStats(ref player_stats);

    }
    void Start()
    {
        player_data.position = InitResources.GetPlayer.GetPosition;
        player_data.speed = player_stats.move_speed;
        shoot_cooldown = 0;
        iframe_cooldown = 0;
        direction = float3.zero;
        multipliers = new PlayerMultipliers(1f, 1f, 1f, 1f);
    }

    public void UpdatePlayer()
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
            InitResources.GetProjectileManager.SpawnProjectile(_projectile_template, data, direction, multipliers.dmg_multiply);
            shoot_cooldown = 1f;

            //InitResources.GetProjectileManager.SpawnProjectile()
        }



        //Debug.Log($"Moving in direction: {player_data.direction}");

        player_data.velocity = player_data.speed * player_data.direction;
        InitResources.GetPlayer.SetVelocity(player_data.velocity);
    }

    public PlayerMultipliers GetMultipliers => multipliers;

    public void SetMultipliers(PlayerMultipliers m) => multipliers = m;
    public void UpdateMoveSpeed() => player_data.speed = player_stats.move_speed * multipliers.ms_multiply;
    public void IncreaseMaxHP(float hp) => player_stats.max_hp += hp;
    public void IncreaseCurrentHP(float hp) => player_stats.current_hp += hp;
    public void SetDamage(float dmg) => multipliers.dmg_multiply += dmg;


    public void TakeDMG(float DMG)
    {
        player_stats.current_hp -= DMG;
        iframe_cooldown = 0.5f;
        InitResources.GetEventChannel.TriggerHealthChange();
    }

    public float GetHealthPercentage => player_stats.current_hp / player_stats.max_hp;

}
