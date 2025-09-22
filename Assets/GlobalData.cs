using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(fileName = "NewGlobalData", menuName = "Data/Global Data")]
public class GlobalData : ScriptableObject
{

    // PLAYER

    // Player Events
    // [HideInInspector] public event Action OnAltShoot;
    // [HideInInspector] public event Action OnAbility;
    // [HideInInspector] public event Action OnMove;
    // [HideInInspector] public event Action OnMoveStop;

    [HideInInspector] public float2 MousePosition;
    [HideInInspector] public float2 PlayerMoveDirection;
    [HideInInspector] public bool IsShooting;
    [HideInInspector] public bool Flip;
    [HideInInspector] public float3 PlayerPosition;


    // INPUT

}
