using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{

    public int ID;
    public float blink_strength;
    public float blink_speed;
    public float DMG;

    private Rigidbody2D rb;
    private SpriteRenderer sprite_renderer;
    private LineRenderer line_renderer;
    private float line_width;


    MaterialPropertyBlock propblock;

    float3 line_offset;





    Transform cache_transform;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        line_renderer = GetComponent<LineRenderer>();
        cache_transform = transform;
    }

    void Start()
    {
        line_width = 0f;
        line_offset = new float3(0f, 0.08f, 0f);
        blink_speed = 4f;
        sprite_renderer.sharedMaterial = GraphicsResources.GetBlinkerFluid;

        //line_renderer.enabled = false;
        propblock = new MaterialPropertyBlock();
    }

    void Update()
    {
        if (blink_strength > 0)
        {
            blink_strength -= Time.deltaTime * blink_speed;
        }
        float lerp = GraphicsResources.GetBlinkerFluidAnimation.Evaluate(Mathf.Clamp01(blink_strength));

        propblock.SetFloat("_BlinkStrength", lerp);
        sprite_renderer.SetPropertyBlock(propblock);
        line_renderer.SetPosition(0, cache_transform.position + (Vector3)line_offset);

        line_width -= Time.deltaTime * 2f;
        line_renderer.widthMultiplier = Mathf.Clamp01(line_width);
    }
    public float3 GetPosition() => cache_transform.position;

    public int GetID() => ID;

    public void SetColor(float3 color) => sprite_renderer.color = new Color(color.x, color.y, color.z);

    public void SetPosition(float3 position) => cache_transform.position = position;
    public void SetVelocity(float3 velocity) => rb.linearVelocity = velocity.xy;
    public void DrawRaycastLine(float3 target) => line_renderer.SetPosition(1, target);


    public void ShowLineRenderer(bool b) => line_renderer.enabled = b;


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerBehaviour>(out PlayerBehaviour collider_ref))
        {
            collider_ref.TakeDMG(DMG);
        }
    }

}
