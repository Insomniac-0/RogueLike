using System;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitResources : MonoBehaviour
{
    public static InitResources Instance;

    [SerializeField]
    private CursorBehaviour cursor_ref;

    private ProjectileManager projectile_manager;
    private EntityManagerBehaviour entity_manager;
    private InputReader input_reader;
    private NullableObjects nullable_objects;
    private CursorBehaviour cursor;
    private SoundManager sound_manager;



    void Awake()
    {
        InitResources.Instance = this;

        projectile_manager = GetComponent<ProjectileManager>();
        entity_manager = GetComponent<EntityManagerBehaviour>();
        input_reader = GetComponent<InputReader>();
        nullable_objects = GetComponent<NullableObjects>();
        sound_manager = GetComponent<SoundManager>();

        cursor = Instantiate(cursor_ref);


        Application.targetFrameRate = 0;
        QualitySettings.vSyncCount = -1;
    }
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(cursor.gameObject);
        SceneManager.LoadSceneAsync(1);
    }

    public static ProjectileManager GetProjectileManager => Instance.projectile_manager;
    public static EntityManagerBehaviour GetEntityManagerBehaviour => Instance.entity_manager;
    public static InputReader GetInputReader => Instance.input_reader;
    public static NullableObjects GetNullableObjects => Instance.nullable_objects;
    public static SoundManager GetSoundManager => Instance.sound_manager;

    public static Camera GetCamera => Instance.nullable_objects.cam;
    public static Player GetPlayer => Instance.nullable_objects.player;
    public static CursorBehaviour GetCursor => Instance.cursor;
}
