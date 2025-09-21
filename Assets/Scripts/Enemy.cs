using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData enemy_blueprint;
    [SerializeField] public float Health;

    public int id;
    private float _current_healh;
    private float _movement_speed;

    private Rigidbody2D rb;
    private SpriteRenderer sprite_renderer;

    void Awake()
    {
        _current_healh = enemy_blueprint.MaxHealth;
        _movement_speed = enemy_blueprint.MovementSpeed;
        rb = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    public float3 GetPosition() => transform.position;

    public void SetColor(float3 color) => sprite_renderer.color = new Color(color.x, color.y, color.z);
    public void SetPosition(float3 position) => transform.position = position;
    public void SetVelocity(float3 velocity) => rb.linearVelocity = velocity.xy;


    public void TakeDamage(float dmg)
    {
        Health -= dmg;
        Debug.Log(dmg + " Taken");
    }
}
