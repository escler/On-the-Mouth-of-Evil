using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaltUI : ItemUI
{
    public Slider slider;

    private void OnEnable()
    {
        _image = GetComponent<Image>();
        slider.maxValue = 10;
    }

    public void SaltUsed(int value)
    {
        slider.value = value;
    }

    public void SetUses(int value, int maxUses)
    {
        slider.maxValue = maxUses;
        slider.value = value;
    }


    private void OnDisable()
    {
    }
}
