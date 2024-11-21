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
    }

    private void Start()
    {
        SearchPrefs();
    }

    void SearchPrefs()
    {
        if (PlayerPrefs.HasKey("FXVolume"))
        {
            float value = PlayerPrefs.GetFloat("FXVolume");
            value = MathF.Log10(value) * 20;
            mixer.SetFloat("FXVolume", value);
        }
        else
        {
            float value = .5f;
            value = Mathf.Log10(value) * 20;
            mixer.SetFloat("FXVolume", value);
        }

        
        if (PlayerPrefs.HasKey("AmbientVolume"))
        {
            float value = PlayerPrefs.GetFloat("AmbientVolume");
            value = MathF.Log10(value) * 20;
            mixer.SetFloat("AmbientVolume", value);
        }
        else
        {
            float value = .5f;
            value = Mathf.Log10(value) * 20;
            mixer.SetFloat("AmbientVolume", value);
        }

    }
}
