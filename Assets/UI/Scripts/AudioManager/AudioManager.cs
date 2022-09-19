using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    public List<Sound> music;
    [SerializeField] private AudioSource audioSource;
    public bool mute;

    public override void Awake()
    {
        if(audioSource == null)
        { 
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        base.Awake();
        foreach (Sound sound in music) {
            sound.source = audioSource;
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
        Play(SoundName.GameTheme);
    }
    private void Start()
    {
        EnventManager.AddListener(EventName.StartGame.ToString(), () => { StopMusic(SoundName.GameTheme); });
    }
    public void Play(SoundName soundName) {
        foreach (Sound sound in music)
        {
            if (sound.audioName == soundName)
            {
                sound.source.Play();
                return;
            }
        }
        Debug.Log("Can't play sound name: "+ soundName.ToString());
    }
    public void StopMusic(SoundName soundName)
    {
        foreach (Sound sound in music)
        {
            if(sound.audioName == soundName)
            {
                sound.source.Stop();
            }
        }
           
    }
    public void SetMuteMusic(bool muteBool) {
        mute = muteBool;
        foreach (Sound sound in music)
            sound.source.mute = mute;
    }
}
