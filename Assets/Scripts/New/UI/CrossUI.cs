using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CrossUI : ItemUI
{
    public Slider slider;
    private CrossCD _crossCd;

    private void OnEnable()
    {
        _image = GetComponent<Image>();
        slider.maxValue = PlayerHandler.Instance.GetComponent<CrossCD>().maxCooldown;
        _crossCd = PlayerHandler.Instance.GetComponent<CrossCD>();
        slider.value = _crossCd.Cooldown;
        _crossCd.OnCrossTimerChange += UpdateSliderValue;
    }
    
    private void UpdateSliderValue()
    {
        slider.value = _crossCd.Cooldown;
    }

    private void OnDisable()
    {
        _image.color = Color.white;
        _crossCd.OnCrossTimerChange -= UpdateSliderValue;
    }
}
