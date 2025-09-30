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
public struct SoundInstance
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
    [SerializeField] List<SoundInstance> sound_instance;

    public void PlaySoundEffect(SoundFX fx)
    {
        for (int i = 0; i < sound_instance.Count; i++)
        {
            if (sound_instance[i].sfx == fx)
            {
                sound_instance[i].PlaySound();
                return;
            }
        }
    }
}
