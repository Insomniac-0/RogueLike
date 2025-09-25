using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] ProjectileManager manager;
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
        if (collision.TryGetComponent<Enemy>(out Enemy collider_ref) && _prevID != collider_ref.GetID())
        {
            _prevID = collider_ref.GetID();
            collider_ref.GetComponent<EntityManagerBehaviour>().TakeDmg(_prevID, 5);
            gameObject.GetComponent<ProjectileManager>().TakeDMG(this.ID);
        }
    }
}
