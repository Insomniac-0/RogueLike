using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    int prevID;

    public float blink_strength;
    public float blink_speed;

    float3 direction;
    Transform cache_transform;
    PlayerBehaviour player_behaviour;
    UpgradeSystem upgrade_system;
    SpriteRenderer sprite_renderer;
    private PlayerAnimation _animation;

    MaterialPropertyBlock propblock;

    private Rigidbody2D rb;

    void Awake()
    {
        blink_speed = 4f;
        player_behaviour = GetComponent<PlayerBehaviour>();
        _animation = GetComponent<PlayerAnimation>();
        upgrade_system = GetComponent<UpgradeSystem>();

        rb = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        InitResources.GetNullableObjects.AssignPlayer(this);
        cache_transform = transform;
    }

    void Start()
    {
        sprite_renderer.sharedMaterial = GraphicsResources.GetBlinkerFluid;
        propblock = new MaterialPropertyBlock();
    }


    public float3 GetPosition => cache_transform.position;

    public PlayerBehaviour GetPlayerBehaviour => player_behaviour;


    public float GetHpPercentage => player_behaviour.GetHealthPercentage;



    public Transform GetTransform => cache_transform;

    public void PlayerUpdate()
    {

        player_behaviour.UpdatePlayer();
        _animation.AnimationUpdate();
        if (blink_strength > 0)
        {
            blink_strength -= Time.deltaTime * blink_speed;
        }
        float lerp = GraphicsResources.GetBlinkerFluidAnimation.Evaluate(Mathf.Clamp01(blink_strength));

        propblock.SetFloat("_BlinkStrength", lerp);
        sprite_renderer.SetPropertyBlock(propblock);
    }


    //public void SetColor(float3 color) => _sprite_renderer.color = new Color(color.x, color.y, color.z);
    public void SetPosition(float3 pos) => cache_transform.position = pos;
    public void SetVelocity(float3 velocity)
    {
        direction = math.normalizesafe(velocity);
        rb.linearVelocity = velocity.xy;
    }

    public float3 GetDirection => direction;

    public void RaycastHit() => Debug.Log("HIT");
}
