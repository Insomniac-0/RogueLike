using UnityEngine;

public class PauseUI : MonoBehaviour
{
    void Awake()
    {
        InitResources.GetNullableObjects.AssignPauseUI(this);
    }
}
