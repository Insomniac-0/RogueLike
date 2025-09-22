using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class InputBehaviour : MonoBehaviour
{
    [SerializeField] GlobalData global_data;
    [SerializeField] Camera cam;

    GameManager game_manager;

    private float2 mouse_position;
    private float2 move_direction;

    private Inputs inputs;

    public event Action OnAltShoot;
    public event Action OnAbility;
    public event Action OnMove;
    public event Action OnMoveStop;

    enum HorizontalDir
    {
        RIGHT,
        LEFT
    }

    public bool is_shooting;

    public bool flip;
    HorizontalDir current;

    void Awake()
    {
        inputs = new Inputs();
        current = HorizontalDir.RIGHT;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        inputs.PlayerActions.MousePosition.performed += (e) => GameData.MousePosition = e.ReadValue<Vector2>();

        inputs.PlayerActions.Move.performed += (e) =>
        {
            GameData.PlayerMoveDirection = e.ReadValue<Vector2>();
            if (move_direction.x < 0 && current != HorizontalDir.LEFT) flip = true;
            if (move_direction.x > 0) flip = false;
            OnMove?.Invoke();
        };

        inputs.PlayerActions.Move.canceled += _ =>
        {
            GameData.PlayerMoveDirection = float2.zero;
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
