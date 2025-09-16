using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputBehaviour : MonoBehaviour
{
    [SerializeField]
    Camera cam;

    private Inputs inputs;
    private float2 mouse_position;
    private float2 move_direction;

    public event Action OnShoot;
    public event Action OnAltShoot;
    public event Action OnAbility;

    void Awake()
    {
        inputs = new Inputs();
        cam = Camera.main;


        inputs.PlayerActions.MousePosition.performed += (e) => mouse_position = e.ReadValue<Vector2>();

        inputs.PlayerActions.Move.performed += (e) => move_direction = e.ReadValue<Vector2>();
        inputs.PlayerActions.Move.canceled += (e) => move_direction = Vector2.zero;


        inputs.PlayerActions.Shoot.performed += _ => OnShoot?.Invoke();
        inputs.PlayerActions.AltShoot.performed += _ => OnAltShoot?.Invoke();
        inputs.PlayerActions.Ability.performed += _ => OnAbility?.Invoke();
    }

    private void OnEnable() => inputs.Enable();
    private void OnDisable() => inputs.Disable();


    public float3 GetMoveDirection() => new float3(move_direction.xy, 0);

    public float2 GetMousePositionSS() => mouse_position;
    public float3 GetMousePositionWS() => cam.ScreenToWorldPoint(new float3(mouse_position.xy, 0f));

}
