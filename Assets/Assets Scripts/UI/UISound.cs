﻿using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISound : MonoBehaviour {

    EventInstance SFXVolumeEvent;
    EventInstance playSelectSound;
    EventInstance backSelectSound;
    EventInstance startGameSound;

    Bus Music;
    Bus SFX;
    Bus Master;
    private float MusicVolume = 0.5f;
    private float SFXVolume = 0.5f;
    private float MasterVolume = 1f;

    void Awake()
    {
        Music = RuntimeManager.GetBus("bus:/Master/Music");
        SFX = RuntimeManager.GetBus("bus:/Master/SFX");
        Master = RuntimeManager.GetBus("bus:/Master");
        SFXVolumeEvent = RuntimeManager.CreateInstance("event:/Master/SFX/UISFX/Neg");
    }
    
    void Start () {
		
	}
		
	void Update () {
        Music.setVolume(MusicVolume);
        SFX.setVolume(SFXVolume);
        Master.setVolume(MasterVolume);
    }

    public void MasterVolumeLevel(float master)
    {
        MasterVolume = master;
    }
    public void MusicVolumeLevel(float music)
    {
        MusicVolume = music;
    }
    public void SFXVolumeLevel(float sfx)
    {
        SFXVolume = sfx;

        PLAYBACK_STATE PbState;
        SFXVolumeEvent.getPlaybackState(out PbState);
        if (PbState != PLAYBACK_STATE.PLAYING)
        {
            SFXVolumeEvent.start();
        }
    }  
}
