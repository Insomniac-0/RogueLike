using System;
using System.Diagnostics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.iOS;
using UnityEngine.Pool;

public class InputReader : MonoBehaviour
{
    //[SerializeField] Float2EventChannel _channel;
    [SerializeField] Camera cam;
    [SerializeField] Player player;



    GameManager game_manager;

    private float2 mouse_position;
    private float2 move_direction;

    private Inputs inputs;

    public event Action OnAltShoot;
    public event Action OnAbility;
    public event Action OnMove;
    public event Action OnMoveStop;


    public enum PlayerMoveDirection
    {
        E,
        NE,
        N,
        NW,
        W,
        SW,
        S,
        SE,
    }
    public PlayerMoveDirection current_direction;
    public bool is_shooting;


    public bool flip;

    void Awake()
    {
        inputs = new Inputs();
        is_shooting = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        inputs.PlayerActions.MousePosition.performed += (e) =>
        {
            mouse_position = e.ReadValue<Vector2>();

            float3 direction = new float3(mouse_position.xy, 0) - player.GetPosition();
            float angle_rad = math.atan2(direction.y, direction.x);
            float angle_deg = angle_rad * (180 / math.PI);

            int dir = (int)(angle_deg / 45.0f) % 8;
            current_direction = (PlayerMoveDirection)dir;
        };
        inputs.PlayerActions.Move.performed += (e) =>
        {
            move_direction = e.ReadValue<Vector2>();
            OnMove?.Invoke();
        };

        inputs.PlayerActions.Move.canceled += _ =>
        {
            move_direction = float2.zero;
            OnMoveStop?.Invoke();
        };


        inputs.PlayerActions.Shoot.started += _ => is_shooting = true;
        inputs.PlayerActions.Shoot.canceled += _ => is_shooting = false;
        inputs.PlayerActions.AltShoot.performed += _ => OnAltShoot?.Invoke();
        inputs.PlayerActions.Ability.performed += _ => OnAbility?.Invoke();
    }

    private void OnEnable() => inputs.PlayerActions.Enable();
    private void OnDisable() => inputs.PlayerActions.Disable();

    public float3 GetMoveDirection() => new(move_direction.xy, 0);

    public float2 GetMousePositionSS() => mouse_position;
    public float3 GetMousePositionWS() => cam.ScreenToWorldPoint(new float3(mouse_position.xy, 0f));


}
