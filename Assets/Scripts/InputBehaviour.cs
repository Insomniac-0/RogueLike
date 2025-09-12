using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputBehaviour : MonoBehaviour
{
    [SerializeField]
    GameObject cursor;

    private Inputs inputs;
    private Vector2 mouse_position;
    private Vector2 move_direction;


    void Awake()
    {
        inputs = new Inputs();


        inputs.PlayerActions.MousePosition.performed += (e) => mouse_position = e.ReadValue<Vector2>();
        inputs.PlayerActions.Move.performed += (e) => move_direction = e.ReadValue<Vector2>();
        inputs.PlayerActions.Move.canceled += (e) => move_direction = Vector2.zero;


    }

    private void OnEnable() => inputs.Enable();
    private void OnDisable() => inputs.Disable();

    public Vector2 GetMoveDirection() => move_direction;

    public Vector2 GetMousePositionSS() => mouse_position;
    public Vector3 GetMousePositionWS() => Camera.main.ScreenToWorldPoint(mouse_position);

}
