using System;
using System.Collections.Generic;
using System.Linq;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using NUnit.Framework.Internal;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public struct SoundData
{
    [SerializeField] public EventReference event_ref;
    [SerializeField] public bool pausable;
}

public delegate void PrematureDespawnTrigger();

public class SoundManager : MonoBehaviour
{
    List<SoundWrapper> sound_wrappers;
    void Awake()
    {
        sound_wrappers = new List<SoundWrapper>();
    }

    void Start()
    {
        InitResources.GetEventChannel.OnSettingsChange += UpdateVolume;
    }
    public PrematureDespawnTrigger SpawnSound(SoundData data)
    {
        SoundWrapper wrapper = new SoundWrapper(data);
        sound_wrappers.Add(wrapper);
        return () =>
        {
            SoundWrapper wrap = wrapper;
            wrapper.Stop();
            wrapper.Dispose();
            sound_wrappers.Remove(wrap);
        };
    }
    public void SoundUpdate()
    {
        for (int i = sound_wrappers.Count - 1; i >= 0; i--)
        {
            sound_wrappers[i].PollState();
            if (sound_wrappers[i].state != PLAYBACK_STATE.PLAYING)
            {
                sound_wrappers[i].Dispose();
                sound_wrappers.RemoveAtSwapBack(i);
            }
        }
    }

    public void UpdateVolume()
    {
        float volume = InitResources.GetGameSettings.GetVolume;
        for (int i = 0; i < sound_wrappers.Count; i++)
        {
            sound_wrappers[i].SetVolume(volume);
        }
    }
}



public class SoundWrapper
{
    EventInstance event_instance;
    public PLAYBACK_STATE state;
    private bool pausable;

    public void PollState()
    {
        event_instance.getPlaybackState(out state);
        bool game_pause;
        game_pause = pausable && InitResources.GetGameManager.IsPaused;
        if (game_pause) event_instance.setPaused(true);
    }

    public SoundWrapper(SoundData data)
    {
        pausable = data.pausable;
        event_instance = RuntimeManager.CreateInstance(data.event_ref);
        event_instance.setVolume(InitResources.GetGameSettings.GetVolume);
        event_instance.start();
    }




    public void SetVolume(float volume)
    {
        event_instance.setVolume(volume);
    }
    public void Stop()
    {
        event_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    public void Dispose()
    {
        event_instance.release();
    }
}