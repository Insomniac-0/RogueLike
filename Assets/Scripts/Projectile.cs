using Unity.Mathematics;
using UnityEditor.Search;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public int hits;

    private SpriteRenderer sprite_renderer;
    public bool Collided;

    public float3 GetPosition() => transform.position;

    public void SetPosition(float3 pos) => transform.position = pos;
    public void SetColor(float3 color) => sprite_renderer.color = new Color(color.x, color.y, color.z);

    void Awake()
    {
        hits = 1;
        sprite_renderer = GetComponent<SpriteRenderer>();
        damage = 2f;
        Collided = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && !Collided)
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
            hits = 0;
            Collided = true;
        }
    }
}
