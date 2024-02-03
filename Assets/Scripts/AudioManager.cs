using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sounds[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        //Starts game with BGM
        PlayMusic("BGM");
    }

    //use these functions outside the script to play desired SFX and Music (using AudioManager.Instance)
    
    public void PlayMusic(string name)
    {
        Sounds s = Array.Find(musicSounds, x=> x.name == name);

        if(s==null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip=s.clip;
            musicSource.Play();
        }
    }
    public void PauseMusic(string name)
    {
        Sounds s = Array.Find(musicSounds, x=> x.name == name);

        if(s==null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip=s.clip;
            musicSource.Pause();
        }
    }
    public void UnPauseMusic(string name)
    {
        Sounds s = Array.Find(musicSounds, x=> x.name == name);

        if(s==null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip=s.clip;
            musicSource.UnPause();
        }
    }
    public void StopMusic(string name)
    {
        Sounds s = Array.Find(musicSounds, x=> x.name == name);

        if(s==null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip=s.clip;
            musicSource.Stop();
        }
    }
    public void PlaySFX(string name)
    {
        Sounds s = Array.Find(sfxSounds, x=> x.name == name);

        if(s==null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }
    
    //adjust volume with slider coming from VolumeController
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

}

