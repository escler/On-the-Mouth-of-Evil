using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AmbientHandler : MonoBehaviour
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
        if (PlayerPrefs.HasKey("AmbientVolume"))
        {
            _slider.value = PlayerPrefs.GetFloat("AmbientVolume");
            PlayerPrefs.Save();
            return;
        }

        _slider.value = .5f;
        PlayerPrefs.SetFloat("AmbientVolume", _slider.value);
        PlayerPrefs.Save();
    }

    private void ChangeValue()
    {
        mixer.SetFloat("AmbientVolume", Mathf.Log10(_slider.value) * 20);
        PlayerPrefs.SetFloat("AmbientVolume", _slider.value);
        PlayerPrefs.Save();
    }
}
