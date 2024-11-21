using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerHandler : MonoBehaviour
{
    public static AudioMixerHandler Instance { get; private set; }
    public AudioMixer mixer;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SearchPrefs();
    }

    private void SearchPrefs()
    {
        if (PlayerPrefs.HasKey("FXVolume"))
        {
            mixer.SetFloat("FXVolume", Mathf.Log10(PlayerPrefs.GetFloat("FXVolume") * 20));
        }
        else
        {
            mixer.SetFloat("FXVolume", Mathf.Log10(1 * 20));
        }

        
        if (PlayerPrefs.HasKey("AmbientFX"))
        {
            mixer.SetFloat("AmbientFX", Mathf.Log10(PlayerPrefs.GetFloat("AmbientFX") * 20));
        }
        else
        {
            mixer.SetFloat("AmbientFX", Mathf.Log10(1 * 20));
        }

    }
}
