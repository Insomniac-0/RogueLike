using Unity.Mathematics;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    Transform cache_transform;

    void Awake()
    {
        cache_transform = transform;
    }

    public float3 GetPosition => cache_transform.position;
}
