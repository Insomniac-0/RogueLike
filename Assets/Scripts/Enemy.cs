using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite_renderer;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    public float3 GetPosition() => transform.position;

    public void SetColor(float3 color) => sprite_renderer.color = new Color(color.x, color.y, color.z);
    public void SetPosition(float3 position) => transform.position = position;
    public void SetVelocity(float3 velocity) => rb.linearVelocity = velocity.xy;


    public void TakeDamage(float dmg)
    {
        Debug.Log(dmg + " Taken");
    }
}
