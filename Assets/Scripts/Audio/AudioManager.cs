using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    void Awake()
    {
        if(instance == null) instance = this;
        else {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;

            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
        }
    }

    private void Start() 
    {
        AudioPlay("Theme");
    }

    public void AudioPlay(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null) 
        {
            Debug.LogWarning("Sound: " + name + "Not found.");
            return;
        }
        s.audioSource.Play();
    }
}
