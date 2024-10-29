using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SensHandler : MonoBehaviour
{
    private Slider _slider;
    public TextMeshProUGUI sensValueText;
    private float _sensValue;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue = 5;
        TakePrefsValue();
        _slider.onValueChanged.AddListener(delegate {SetValue();});
    }

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(delegate {SetValue();});
    }

    private void SetValue()
    {
        print("Me llame");
        PlayerHandler.Instance.playerCam.sens = _slider.value;
        sensValueText.text = _slider.value.ToString("0.00");
        PlayerPrefs.SetFloat("Sens", _slider.value);
        PlayerPrefs.Save();
    }
    private void TakePrefsValue()
    {
        if (PlayerPrefs.HasKey("Sens"))
        {
            _slider.value = PlayerPrefs.GetFloat("Sens");
            sensValueText.text = _slider.value.ToString("0.00");
            PlayerPrefs.Save();
            return;
        }

        _slider.value = 1;
        PlayerPrefs.SetFloat("Sens", _slider.value);
        sensValueText.text = _slider.value.ToString("0.00");
        PlayerPrefs.Save();
    }
}
