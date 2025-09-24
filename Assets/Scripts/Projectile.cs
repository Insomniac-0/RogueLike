using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour _player;
    public int ID;

    private SpriteRenderer sprite_renderer;
    private int _prevID;

    public float3 GetPosition() => transform.position;

    public void SetPosition(float3 pos) => transform.position = pos;
    public void SetColor(float3 color) => sprite_renderer.color = new Color(color.x, color.y, color.z);

    void Awake()
    {
        _prevID = -1;
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && collision.GetComponent<Enemy>().ID != _prevID)
        {
            _prevID = collision.GetComponent<Enemy>().ID;
            //collision.GetComponent<Enemy>().TakeDamage(damage);
            collision.GetComponent<EnemySystem>().AddCollisionEvent(this.ID, collision.GetComponent<Enemy>().ID);
        }

    }
}
