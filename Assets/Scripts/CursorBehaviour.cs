using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorBehaviour : MonoBehaviour
{
    [SerializeField]
    Camera cam;

    [SerializeField]
    InputBehaviour input_behaviour;

    [SerializeField] RectTransform cursor_sprite;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
    void Update()
    {
        UnityEngine.Vector2 mouse_SS = input_behaviour.GetMousePositionSS();
        UnityEngine.Vector3 mouse_WS = cam.ScreenToWorldPoint(new UnityEngine.Vector3(mouse_SS.x, mouse_SS.y, cam.nearClipPlane));
        mouse_WS.z = 0f;
        transform.position = mouse_WS;
        cursor_sprite.position = mouse_WS;
    }
}
