using Unity.Mathematics;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;


    private SpriteRenderer sprite_renderer;

    public float3 GetPosition() => transform.position;

    public void SetPosition(float3 pos) => transform.position = pos;
    public void SetColor(float3 color) => sprite_renderer.color = new Color(color.x, color.y, color.z);
    public void SetVelocity(float3 velocity) => rb.linearVelocity = velocity.xy;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }
}
