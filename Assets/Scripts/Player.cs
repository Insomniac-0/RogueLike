using System;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] InputBehaviour input_behaviour;

    public float3 GetPosition() => transform.position;

    public void SetPosition(float3 pos) => transform.position = pos;
    public void SetColor(float3 color) => GetComponent<SpriteRenderer>().color = new Color(color.x, color.y, color.z);

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
        Console.WriteLine("Shoot");
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
