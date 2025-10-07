using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    int prevID;
    public float Health;
    float3 direction;
    Transform cache_transform;
    PlayerBehaviour player_behaviour;
    UpgradeSystem upgrade_system;
    SpriteRenderer sprite_renderer;

    private Rigidbody2D rb;

    void Awake()
    {
        player_behaviour = GetComponent<PlayerBehaviour>();
        upgrade_system = GetComponent<UpgradeSystem>();

        rb = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        InitResources.GetNullableObjects.AssignPlayer(this);
        cache_transform = transform;
    }

    void Start()
    {
        sprite_renderer.sharedMaterial = GraphicsResources.GetBlinkerFluid;
    }

    public float3 GetPosition => cache_transform.position;

    public PlayerBehaviour GetPlayerBehaviour => player_behaviour;


    public float GetHpPercentage => player_behaviour.GetHealthPercentage;



    public Transform GetTransform => cache_transform;

    public void PlayerUpdate() => player_behaviour.UpdatePlayer();

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
