using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public sealed class Projectile : MonoBehaviour
{
    public int ID;
    Transform trans_cache;

    private SpriteRenderer sprite_renderer;
    private int _prevID;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float3 GetPosition() => trans_cache.position;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetPosition(in float3 pos) => trans_cache.position = pos;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetRotation(in Quaternion rot) => trans_cache.rotation = rot;


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetColor(float3 color) => sprite_renderer.color = new Color(color.x, color.y, color.z);

    void Awake()
    {
        _prevID = -1;
        sprite_renderer = GetComponent<SpriteRenderer>();
        trans_cache = transform;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy collider_ref) && _prevID != collider_ref.GetID())
        {
            _prevID = collider_ref.GetID();
            InitResources.GetEntityManagerBehaviour.TakeDmg(_prevID, 5);
            InitResources.GetProjectileManager.TakeDMG(ID);
            // collider_ref.GetComponent<EntityManagerBehaviour>().TakeDmg(_prevID, 5);
            // gameObject.GetComponent<ProjectileManager>().TakeDMG(this.ID);
        }
    }
}
