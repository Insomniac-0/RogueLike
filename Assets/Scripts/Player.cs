using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] InputBehaviour input_behaviour;

    private Rigidbody2D rb;
    private SpriteRenderer sprite_renderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    public float3 GetPosition() => transform.position;

    public void SetColor(float3 color) => sprite_renderer.color = new Color(color.x, color.y, color.z);
    public void SetPosition(float3 pos) => transform.position = pos;
    public void SetVelocity(float3 velocity) => rb.linearVelocity = velocity.xy;


    private void OnEnable()
    {
        input_behaviour.OnShoot += Shoot;
        input_behaviour.OnAltShoot += AltShoot;
        input_behaviour.OnAbility += UseAbility;
    }

    private void OnDisable()
    {
        input_behaviour.OnShoot -= Shoot;
        input_behaviour.OnAltShoot -= AltShoot;
        input_behaviour.OnAbility -= UseAbility;
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
