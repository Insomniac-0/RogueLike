using System.Security.Cryptography;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollowBehaviour : MonoBehaviour
{
    [SerializeField][Range(0.1f, 20f)] float speed;
    [SerializeField][Range(.0f, 2.0f)] float intensity;
    Player player;
    Transform cache_transform;
    float3 camera_offset;
    float3 smooth_direction;

    void Awake()
    {
        cache_transform = transform;
    }
    void Start()
    {
        player = InitResources.GetPlayer;
        camera_offset = new float3(0f, 0f, -10f);
    }
    void Update()
    {
        if (!InitResources.GetPlayer) return;
        float3 player_position = player.GetPosition;
        float3 move_dir = player.GetDirection;

        smooth_direction = math.lerp(smooth_direction, move_dir * intensity, Time.deltaTime * speed);

        float3 accum = player_position + smooth_direction + camera_offset;



        cache_transform.position = accum;
    }
}

