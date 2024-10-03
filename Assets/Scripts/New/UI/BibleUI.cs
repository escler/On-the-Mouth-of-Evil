using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BibleUI : ItemUI
{
    public Slider slider;
    private BibleCD _bibleCD;

    private void OnEnable()
    {
        _image = GetComponent<Image>();
        slider.maxValue = 10;
        _bibleCD = PlayerHandler.Instance.GetComponent<BibleCD>();
        slider.value = _bibleCD.Cooldown;
        _bibleCD.OnBibleTimerChange += UpdateSliderValue;
    }
    
    private void UpdateSliderValue()
    {
        slider.value = _bibleCD.Cooldown;
    }

    private void OnDisable()
    {
        _image.color = Color.white;
        _bibleCD.OnBibleTimerChange -= UpdateSliderValue;
    }
}
