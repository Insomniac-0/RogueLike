using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float Health;
    Transform cache_transform;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        InitResources.GetNullableObjects.AssignPlayer(this);
        cache_transform = transform;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float3 GetPosition() => cache_transform.position;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Transform GetTransform() => cache_transform;

    //public void SetColor(float3 color) => _sprite_renderer.color = new Color(color.x, color.y, color.z);
    public void SetPosition(float3 pos) => cache_transform.position = pos;
    public void SetVelocity(float3 velocity) => rb.linearVelocity = velocity.xy;
}
