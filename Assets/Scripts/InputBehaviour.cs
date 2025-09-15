using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputBehaviour : MonoBehaviour
{
    [SerializeField]
    Camera cam;

    private Inputs inputs;
    private Vector2 mouse_position;
    private Vector2 move_direction;

    public event Action OnShoot;
    public event Action OnAltShoot;
    public event Action OnAbility;

    void Awake()
    {
        inputs = new Inputs();


        inputs.PlayerActions.MousePosition.performed += (e) => mouse_position = e.ReadValue<Vector2>();

        inputs.PlayerActions.Move.performed += (e) => move_direction = e.ReadValue<Vector2>();
        inputs.PlayerActions.Move.canceled += (e) => move_direction = Vector2.zero;


        inputs.PlayerActions.Shoot.performed += _ => OnShoot?.Invoke();
        inputs.PlayerActions.AltShoot.performed += _ => OnAltShoot?.Invoke();
        inputs.PlayerActions.Ability.performed += _ => OnAbility?.Invoke();
    }

    private void OnEnable() => inputs.Enable();
    private void OnDisable() => inputs.Disable();


    public Vector2 GetMoveDirection() => move_direction;

    public Vector2 GetMousePositionSS() => mouse_position;
    public float3 GetMousePositionWS() => cam.ScreenToWorldPoint(new float3(mouse_position.x, mouse_position.y, 0f));

}
