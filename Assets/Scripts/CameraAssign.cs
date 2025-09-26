using UnityEngine;

public class CameraAssign : MonoBehaviour
{
    void Awake()
    {
        InitResources.GetNullableObjects.AssignCamera(GetComponent<Camera>());
    }
}
