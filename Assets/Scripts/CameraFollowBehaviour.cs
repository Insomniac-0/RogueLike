using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

public class CameraFollowBehaviour : MonoBehaviour
{
    Player player;


    void Awake()
    {
        InitResources.GetNullableObjects.AssignCamera(GetComponent<Camera>());
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

