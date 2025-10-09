using UnityEngine;
using UnityEngine.UI;

public class VsyncToggle : MonoBehaviour
{
    Toggle toggle;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
        InitResources.GetNullableObjects.AssignVsyncToggle(toggle);
    }
}
