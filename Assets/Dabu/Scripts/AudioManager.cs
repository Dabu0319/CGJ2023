using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
        
        
        foreach (Sound s in sounds)
        {
            s.source=gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        this.Play("BGM");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning(("Sound: "+name+" not found!"));
            return;
        }
            
        s.source.Play();
    }
    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning(("Sound: "+name+" not found!"));
            return;
        }
            
        s.source.Pause();
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning(("Sound: "+name+" not found!"));
            return;
        }
            
        s.source.Stop();
    }
}