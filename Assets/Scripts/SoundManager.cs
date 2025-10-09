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


    public PrematureDespawnTrigger SpawnSound(in SoundData data)
    {
        SoundWrapper wrapper = new SoundWrapper(data);
        sound_wrappers.Add(wrapper);
        // return () =>
        // {
        //     if (sound_wrappers.Contains(wrapper))
        //     {
        //         wrapper.Dispose();
        //         sound_wrappers.Remove(wrapper);
        //     }
        // };
        return PrematureDespawn;
        void PrematureDespawn()
        {
            if (sound_wrappers.Contains(wrapper))
            {
                wrapper.Dispose();
                sound_wrappers.Remove(wrapper);
            }
        }
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
        event_instance.setPaused(game_pause);
    }

    public SoundWrapper(in SoundData data)
    {
        pausable = data.pausable;
        event_instance = RuntimeManager.CreateInstance(data.event_ref);
        PlaySound();
    }


    public void Dispose()
    {
        if (event_instance.isValid()) event_instance.release();
    }
    public void PlaySound()
    {
        event_instance.setTimelinePosition(0);
        event_instance.start();
    }
}