using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioEmitter : MonoBehaviour
{
    [SerializeField] EventReference audio_event;
    EventInstance event_instance;


    void Awake()
    {
        event_instance = RuntimeManager.CreateInstance(audio_event);
    }
    void Start()
    {
        PlaySound();
    }
    public void PlaySound()
    {
        event_instance.setTimelinePosition(0);
        event_instance.start();
    }
    void OnDestroy()
    {
        if (event_instance.isValid()) event_instance.release();
    }
    void AudioEmitterUpdate()
    {
        event_instance.setParameterByName("ReverbStrength", 0f);
    }

    void Update()
    {
        AudioEmitterUpdate();
    }
}

