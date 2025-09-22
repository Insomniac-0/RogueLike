using System;
using Unity.Mathematics;
using UnityEditorInternal;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private Float3Variable player_position;

    [SerializeField] private Player _player;
    [SerializeField] private InputReader _input;
    [SerializeField] private float _initial_speed;


    // TYPES
    struct PlayerData
    {
        public float3 position;
        public float3 direction;
        public float3 velocity;
        public float speed;
    }


    PlayerData player_data;

    void Awake()
    {
        player_data.position = _player.GetPosition();
        player_data.speed = _initial_speed;
        player_data.direction.z = 0;
    }
    void Update()
    {
        Debug.Log($"Moving in direction: {player_data.direction}");
        player_data.position = _player.GetPosition();
        player_position.Value = player_data.position;

        player_data.direction = math.normalizesafe(_input.GetMoveDirection());
        player_data.velocity = player_data.speed * player_data.direction;
        _player.SetVelocity(player_data.velocity);
    }
    public void OnMove(float2 value)
    {
        player_data.direction.xy = value;
    }
}
