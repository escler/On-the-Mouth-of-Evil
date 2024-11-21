using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class FXHandler : MonoBehaviour
{
    public AudioMixer mixer;
    private Slider _slider;
    private void Awake()
    {
        _slider = GetComponent<Slider>();
        TakePrefsValue();
        _slider.onValueChanged.AddListener(delegate { ChangeValue(); });
    }

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(delegate { ChangeValue(); });

    }

    private void TakePrefsValue()
    {
        if (PlayerPrefs.HasKey("FXVolume"))
        {
            _slider.value = PlayerPrefs.GetFloat("FXVolume");
            PlayerPrefs.Save();
            return;
        }

        _slider.value = .5f;
        PlayerPrefs.SetFloat("FXVolume", _slider.value);
        PlayerPrefs.Save();
    }

    private void ChangeValue()
    {
        mixer.SetFloat("FXVolume", Mathf.Log10(_slider.value) * 20);
        PlayerPrefs.SetFloat("FXVolume", _slider.value);
        PlayerPrefs.Save();
    }
}
