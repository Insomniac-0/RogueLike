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

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player_behaviour = GetComponent<PlayerBehaviour>();
        InitResources.GetNullableObjects.AssignPlayer(this);
        cache_transform = transform;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float3 GetPosition() => cache_transform.position;

    public float GetHP() => player_behaviour.player_stats.current_hp;
    public float GetMaxHP() => player_behaviour.player_stats.max_health;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Transform GetTransform() => cache_transform;

    //public void SetColor(float3 color) => _sprite_renderer.color = new Color(color.x, color.y, color.z);
    public void SetPosition(float3 pos) => cache_transform.position = pos;
    public void SetVelocity(float3 velocity) => rb.linearVelocity = velocity.xy;

    public void TakeDamage(float dmg)
    {
        Debug.Log($"{dmg} taken");
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision from Player");
        // if (collision.gameObject.TryGetComponent<Enemy>(out Enemy collider_ref))
        // {
        //     prevID = collider_ref.GetID();
        //     float dmg = InitResources.GetEntityManagerBehaviour.enemies[prevID].dmg;
        //     player_behaviour.TakeDMG(dmg, collider_ref.collider_type);

        // }
    }
}
