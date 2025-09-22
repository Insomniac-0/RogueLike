using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Float2 Event Channel")]
public class Float2EventChannel : ScriptableObject
{
    public event UnityAction<float2> OnEventRaised;

    public void Raise(float2 value)
    {
        if (OnEventRaised != null) OnEventRaised.Invoke(value);
    }
}
