using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public enum SoundName
{
    GameTheme,
    PlayerSound
}
[System.Serializable]
public class Sound
{
    public string nameString;
    public SoundName audioName;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
    public bool loop = false;

    [HideInInspector]
    public AudioSource source;
}
