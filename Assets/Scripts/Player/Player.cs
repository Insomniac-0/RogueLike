using System;
using System.Runtime.CompilerServices;
using GameUtilities.Sprites;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float Health;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }
    public float3 GetPosition() => transform.position;
    //public void SetColor(float3 color) => _sprite_renderer.color = new Color(color.x, color.y, color.z);
    public void SetPosition(float3 pos) => transform.position = pos;
    public void SetVelocity(float3 velocity) => rb.linearVelocity = velocity.xy;
}
