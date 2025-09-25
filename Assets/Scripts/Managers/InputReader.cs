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

    public float angle;
    public int dir;


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

            float3 mouse_ws = cam.ScreenToWorldPoint(new float3(mouse_position.xy, 0));
            float2 direction = math.normalizesafe(mouse_ws.xy - player.GetPosition().xy);
            float angle_rad = math.atan2(direction.y, direction.x);
            angle = math.degrees(angle_rad);
            if (angle < 0) angle += 360;
            dir = ((int)(angle / 45.0f)) % 8;
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
