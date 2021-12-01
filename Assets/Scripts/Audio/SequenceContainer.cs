using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceContainer : MonoBehaviour
{
    private int currentClip;

    public AudioClip[] clips;

    private AudioSource source;

    private void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
    }

    public void Play()
    {
        if (clips.Length > 0)
        {
            source.clip = clips[currentClip];
            source.Play();
            IncreaseClip();
        }
    }

    private void IncreaseClip()
    {
        currentClip++;
        if (currentClip >= clips.Length)
            currentClip = 0;
    }
}
