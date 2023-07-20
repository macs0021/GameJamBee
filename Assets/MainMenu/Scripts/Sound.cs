using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [SerializeField] private string name;

    [SerializeField] private AudioClip clip;

    [SerializeField] private bool loop;

    [Range(0f, 1f), SerializeField]
    private float volume = 0.5f;

    [Range(0.1f, 3f), SerializeField]
    private float pitch = 1f;

    private AudioSource source;

    public string Name { get => name; set => name = value; }
    public AudioClip Clip { get => clip; set => clip = value; }
    public float Volume { get => volume; set => volume = value; }
    public float Pitch { get => pitch; set => pitch = value; }
    public AudioSource Source { get => source; set => source = value; }
    public bool Loop { get => loop; set => loop = value; }
}
