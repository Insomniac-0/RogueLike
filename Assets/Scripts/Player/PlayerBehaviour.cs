using System;
using Unity.Mathematics;
using Unity.VisualScripting.FullSerializer;
using UnityEditorInternal;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private Float3Variable player_position;

    [SerializeField] private Player _player;
    [SerializeField] private InputReader _input;
    [SerializeField] private float _initial_speed;


    // TYPES

    struct PlayerStats
    {

    }

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
        player_data.direction = _input.GetMoveDirection();
        player_data.speed = _initial_speed;
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
}
