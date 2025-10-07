using System;
using JetBrains.Annotations;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitResources : MonoBehaviour
{
    public static InitResources Instance;

    [SerializeField]
    private CursorBehaviour cursor_ref;

    private ProjectileManager projectile_manager;
    private EnemyManagerBehaviour enemy_manager;
    private InputReader input_reader;
    private NullableObjects nullable_objects;
    private CursorBehaviour cursor;
    private SoundManager sound_manager;
    private VfxManager vfx_manager;
    private CollisionMasks collision_masks;
    private UpgradeSystem upgrade_system;
    private EventChannel event_channel;
    private GraphicsResources graphics_resources;
    private GameManager game_manager;

    public struct GameSettings
    {
        public float volume;
        public bool vsync;
        public bool fullscreen;
    }

    void Awake()
    {
        InitResources.Instance = this;

        projectile_manager = GetComponent<ProjectileManager>();
        enemy_manager = GetComponent<EnemyManagerBehaviour>();
        input_reader = GetComponent<InputReader>();
        nullable_objects = GetComponent<NullableObjects>();
        sound_manager = GetComponent<SoundManager>();
        vfx_manager = GetComponent<VfxManager>();
        collision_masks = GetComponent<CollisionMasks>();
        upgrade_system = GetComponent<UpgradeSystem>();
        event_channel = GetComponent<EventChannel>();
        graphics_resources = GetComponent<GraphicsResources>();
        game_manager = GetComponent<GameManager>();

        cursor = Instantiate(cursor_ref);

        Application.targetFrameRate = 0;
        QualitySettings.vSyncCount = -1;

    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadSceneAsync(1);
    }

    public static ProjectileManager GetProjectileManager => Instance.projectile_manager;
    public static EnemyManagerBehaviour GetEnemyManagerBehaviour => Instance.enemy_manager;
    public static InputReader GetInputReader => Instance.input_reader;
    public static NullableObjects GetNullableObjects => Instance.nullable_objects;
    public static SoundManager GetSoundManager => Instance.sound_manager;
    public static VfxManager GetVfxManager => Instance.vfx_manager;
    public static CollisionMasks GetCollisionMasks => Instance.collision_masks;
    public static CursorBehaviour GetCursor => Instance.cursor;
    public static UpgradeSystem GetUpgradeSystem => Instance.upgrade_system;
    public static EventChannel GetEventChannel => Instance.event_channel;
    public static GraphicsResources GetGraphicsResources => Instance.graphics_resources;
    public static GameManager GetGameManager => Instance.game_manager;

    public static Camera GetCamera => GetNullableObjects.cam;
    public static Player GetPlayer => GetNullableObjects.player;
    public static Int32 GetPlayerMask => GetCollisionMasks.GetPlayerMask;

    public static void Init()
    {
        Instance.projectile_manager.Init();
        Instance.enemy_manager.Init();
        Instance.vfx_manager.Init();
        Instance.upgrade_system.Init();
    }

    public static void HardCleanUp()
    {
        Instance.projectile_manager.HardCleanUp();
        Instance.enemy_manager.HardCleanUp();
        Instance.vfx_manager.CleanUp();
    }
    public static void SoftCleanUp()
    {
        Instance.projectile_manager.SoftCleanUp();
        Instance.enemy_manager.SoftCleanUp();
        Instance.vfx_manager.CleanUp();
    }

}
