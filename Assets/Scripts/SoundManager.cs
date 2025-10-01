using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

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
    AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayMusic() => source.Play();
}


