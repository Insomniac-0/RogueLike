using System;
using UnityEngine;

public class EventChannel : MonoBehaviour
{
    // PLAYER EVENTS
    public event Action OnHealthChange;
    public event Action OnXpChange;
    public event Action OnLvlUp;
}
