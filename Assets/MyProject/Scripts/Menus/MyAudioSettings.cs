// Modificaciones por: Jorge Aranda
// jaranlopz@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Esta clase se encarga del volumen de los grupos: Música, SFX y Master.
/// https://scottgamesounds.com/wp-content/uploads/2018/12/C.AudioSettings.txt
/// </summary>
public class MyAudioSettings : MonoBehaviour
{
    FMOD.Studio.EventInstance SFXVolumeTestEvent;

    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus SFX;
    FMOD.Studio.Bus Master;
    float MusicVolume = 0.5f;
    float SFXVolume = 0.5f;
    float MasterVolume = 1f;

    void Awake()
    {
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        SFXVolumeTestEvent = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/SFXVolumeTest");
    }

    private void Start()
    {
        MasterVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
        MusicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        SFXVolume = PlayerPrefs.GetFloat("sfxVolume", 0.5f);
    }

    void Update()
    {
        Master.setVolume(MasterVolume);
        Music.setVolume(MusicVolume);
        SFX.setVolume(SFXVolume);
    }

    public void MasterVolumeLevel(float newMasterVolume)
    {
        MasterVolume = newMasterVolume;
        PlayerPrefs.SetFloat("masterVolume", newMasterVolume);
    }

    public void MusicVolumeLevel(float newMusicVolume)
    {
        MusicVolume = newMusicVolume;
        PlayerPrefs.SetFloat("musicVolume", newMusicVolume);
    }

    public void SFXVolumeLevel(float newSFXVolume)
    {
        SFXVolume = newSFXVolume;
        PlayerPrefs.SetFloat("sfxVolume", newSFXVolume);

        FMOD.Studio.PLAYBACK_STATE PbState;
        SFXVolumeTestEvent.getPlaybackState(out PbState);
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            SFXVolumeTestEvent.start();
        }
    }
}
