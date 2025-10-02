using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    int prevID;
    public float Health;
    Transform cache_transform;
    PlayerBehaviour player_behaviour;
    UpgradeSystem upgrade_system;

    private Rigidbody2D rb;

    void Awake()
    {
        player_behaviour = GetComponent<PlayerBehaviour>();
        upgrade_system = GetComponent<UpgradeSystem>();

        rb = GetComponent<Rigidbody2D>();
        InitResources.GetNullableObjects.AssignPlayer(this);
        cache_transform = transform;


    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float3 GetPosition() => cache_transform.position;

    public float GetHP => player_behaviour.player_stats.current_hp;

    public float GetMaxHP => player_behaviour.player_stats.max_health;

    public float GetHealthPercentage => GetHP / GetMaxHP;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Transform GetTransform() => cache_transform;

    public void AddXP(float xp) => InitResources.GetUpgradeSystem.AddExperience(xp);

    //public void SetColor(float3 color) => _sprite_renderer.color = new Color(color.x, color.y, color.z);
    public void SetPosition(float3 pos) => cache_transform.position = pos;
    public void SetVelocity(float3 velocity) => rb.linearVelocity = velocity.xy;

    public void TakeDamage(float dmg)
    {
        Debug.Log($"{dmg} taken");
    }


    public void RaycastHit()
    {
        Debug.Log("HIT");
    }


}
