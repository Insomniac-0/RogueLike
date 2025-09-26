using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{

    public int ID;

    private Rigidbody2D rb;
    private SpriteRenderer sprite_renderer;

    Transform cache_transform;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        cache_transform = transform;
    }

    void Start()
    {

    }

    public float3 GetPosition() => cache_transform.position;

    public int GetID() => ID;

    public void SetColor(float3 color) => sprite_renderer.color = new Color(color.x, color.y, color.z);
    public void SetPosition(float3 position) => cache_transform.position = position;
    public void SetVelocity(float3 velocity) => rb.linearVelocity = velocity.xy;
}
