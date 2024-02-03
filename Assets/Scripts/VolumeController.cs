using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] Slider MasterVolumeSlider;
    public Slider bgmusicSlider, sfxSlider;

    //Once restart, Volume still persists
    void Start()
    {
        //Set slider to max for new players
        if(!PlayerPrefs.HasKey("MasterVolume") && !PlayerPrefs.HasKey("BGMVolume") && !PlayerPrefs.HasKey("SFXVolume")) 
        {
            PlayerPrefs.SetFloat("MasterVolume",1);
            PlayerPrefs.SetFloat("BGMVolume",1);
            PlayerPrefs.SetFloat("SFXVolume",1);
            Load();
        }
        else
        {
            //straight to Load the saved volume for non-new players
            Load();
        }  
    }

//BG Music Volume
public void MusicVolume()
{
    AudioManager.Instance.MusicVolume(bgmusicSlider.value);
    Save();
}
//Sound Effects Volume
public void SFXVolume()
{
    AudioManager.Instance.SFXVolume(sfxSlider.value);
    Save();
}
//Master Volume
public void MasterVolume()
{
    AudioListener.volume = MasterVolumeSlider.value;
    Save();
}

    //Loads the Volume value
    private void Load()
    {
        float bgm=PlayerPrefs.GetFloat("BGMVolume");
        float sfx=PlayerPrefs.GetFloat("SFXVolume");
        MasterVolumeSlider.value=PlayerPrefs.GetFloat("MasterVolume");
        bgmusicSlider.value=bgm;
        sfxSlider.value=sfx;
    }

    
    //Saves the Volume value
    private void Save()
    {
        PlayerPrefs.SetFloat("MasterVolume", MasterVolumeSlider.value);
        PlayerPrefs.SetFloat("BGMVolume", bgmusicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    }
}
