using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerMini : MonoBehaviour
{

    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
            s.source.maxDistance = s.maxDistance;
            s.source.rolloffMode = s.rollOffMode;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " could not be found!");
            return;
        }
        s.source.Play();
    }

    public void PlayRandom()
    {
        Sound s = sounds[UnityEngine.Random.Range(0, sounds.Length)];
        if (s == null)
        {
            Debug.LogWarning("Random sound could not be found!");
            return;
        }
        s.source.Play();
    }
}
