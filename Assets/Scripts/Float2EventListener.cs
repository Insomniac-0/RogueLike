using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class Float2EventListener : MonoBehaviour
{
    [SerializeField] private Float2EventChannel _channel;
    [SerializeField] private UnityEvent<float2> _OnEventRaised;


    private void OnEnable()
    {
        _channel.OnEventRaised += Respond;
    }

    private void OnDisable()
    {
        _channel.OnEventRaised -= Respond;
    }

    private void Respond(float2 value)
    {
        _OnEventRaised.Invoke(value);
    }
}
