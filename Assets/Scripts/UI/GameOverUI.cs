using System;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    void Awake()
    {
        InitResources.GetNullableObjects.AssignGameOverUI(this);
    }
}
