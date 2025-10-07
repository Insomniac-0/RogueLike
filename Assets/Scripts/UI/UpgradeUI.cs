using Unity.VisualScripting;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    void Awake()
    {
        InitResources.GetNullableObjects.AssignUpgradeUI(this);
    }
}
