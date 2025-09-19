using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] InputBehaviour input_behaviour;
    [SerializeField] Sprite[] sprites;

    private Rigidbody2D rb;
    private SpriteRenderer sprite_renderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        input_behaviour.OnMove += UpdateDirection;
    }

    private void OnEnable()
    {
        input_behaviour.OnAltShoot += AltShoot;
        input_behaviour.OnAbility += UseAbility;
    }

    private void OnDisable()
    {
        input_behaviour.OnAltShoot -= AltShoot;
        input_behaviour.OnAbility -= UseAbility;
    }

    public float3 GetPosition() => transform.position;
    public void SetColor(float3 color) => sprite_renderer.color = new Color(color.x, color.y, color.z);
    public void SetPosition(float3 pos) => transform.position = pos;
    public void SetVelocity(float3 velocity) => rb.linearVelocity = velocity.xy;


    public void UpdateDirection()
    {
        sprite_renderer.sprite = sprites[(int)input_behaviour.player_direction];
        sprite_renderer.flipX = input_behaviour.flip;
    }
    public void Shoot()
    {
        Debug.Log("Shoot");
    }

    public void AltShoot()
    {
        Debug.Log("AltShoot");
    }

    public void UseAbility()
    {
        Debug.Log("Ability Used");
    }

    public void TakeDamage(float dmg)
    {
        Debug.Log(dmg + " Taken");
    }
}
