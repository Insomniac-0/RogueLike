using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

public class CameraFollowBehaviour : MonoBehaviour
{
    [SerializeField] Player p;
    void Update()
    {
        transform.position = new float3(p.GetPosition().xy, -10f);
    }
}

