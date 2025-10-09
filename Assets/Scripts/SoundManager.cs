using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundFX
{
    PLAYER_SHOOT,
    PLAYER_DAMAGED,
    ENEMY_DAMAGED,
    ENEMY_DEATH
}

[Serializable]
public struct Audio
{
    public SoundFX sfx;
    [SerializeField] AudioSource source;

    public void PlaySound()
    {
        source.Play();
    }
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioResource[] audio_resources;
    AudioSource audio_source;

    void Awake()
    {
        audio_source = GetComponent<AudioSource>();
    }

    void Start()
    {
        audio_source.volume = InitResources.GetGameSettings.GetVolume;
        audio_source.resource = audio_resources[1];
        audio_source.loop = true;
        audio_source.Play();
    }

}


