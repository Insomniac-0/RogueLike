using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public sealed class Projectile : MonoBehaviour
{
    [SerializeField] Sprite[] collision_vfx;
    public int ID;
    public int _prevID;
    Transform cache_transform;


    private SpriteRenderer sprite_renderer;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float3 GetPosition() => cache_transform.position;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetPosition(in float3 pos) => cache_transform.position = pos;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetRotation(in Quaternion rot) => cache_transform.rotation = rot;


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetColor(float3 color) => sprite_renderer.color = new Color(color.x, color.y, color.z);


    void Awake()
    {
        _prevID = -1;
        sprite_renderer = GetComponent<SpriteRenderer>();
        cache_transform = transform;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy collider_ref) && _prevID != collider_ref.GetID())
        {
            _prevID = collider_ref.GetID();
            collider_ref.blink_strength = 1f;
            InitResources.GetEnemyManagerBehaviour.TakeDmg(_prevID, 5);
            InitResources.GetProjectileManager.TakeDMG(ID);
            InitResources.GetVfxManager.SpawnAnimation(cache_transform.position, collision_vfx, 6f);
            // collider_ref.GetComponent<EnemyManagerBehaviour>().TakeDmg(_prevID, 5);
            // gameObject.GetComponent<ProjectileManager>().TakeDMG(this.ID);
        }
    }
}
