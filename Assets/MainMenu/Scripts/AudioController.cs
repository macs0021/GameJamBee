using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : PersistentSingleton<AudioController>
{
    [SerializeField] private Sound[] sounds;

    private void Awake()
    {
        base.Awake();

        foreach(Sound sound in sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.Clip;

            sound.Source.volume = sound.Volume;
            sound.Source.pitch = sound.Pitch;
            sound.Source.loop = sound.Loop;
        }

        Play("Main");
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, (s) => s.Name == name);

        if(sound == null)
            return;

        sound.Source.Play();
    }
}
