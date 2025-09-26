using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

public class CameraFollowBehaviour : MonoBehaviour
{
    Player player;
    Transform cache_transform;

    void Awake()
    {
        cache_transform = transform;
    }
    void Start()
    {
        player = InitResources.GetPlayer;
    }
    void Update()
    {
        transform.position = new float3(player.GetPosition().xy, -10f);
    }
}

