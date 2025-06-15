using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightExposureHandlerSlider : MonoBehaviour
{
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
        if (PlayerPrefs.HasKey("LightExposure"))
        {
            _slider.value = PlayerPrefs.GetFloat("LightExposure");
            PlayerPrefs.Save();
            return;
        }

        _slider.value = 0;
        PlayerPrefs.SetFloat("LightExposure", _slider.value);
        PlayerPrefs.Save();
    }
    
    private void ChangeValue()
    {
        LightExposureHandler.Instance.ChangeLightExposure(_slider.value);
    }
}
