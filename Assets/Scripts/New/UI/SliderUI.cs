using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderUI : MonoBehaviour
{
    private Image _image;
    private CrossCD _crossCd;
    private BibleCD _bibleCd;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void SubscribeToCrossEvent()
    {
        _crossCd = PlayerHandler.Instance.GetComponent<CrossCD>();
        _crossCd.OnCrossTimerChange += UpdateSliderCrossValue;
    }

    public void UnSubscribeToCrossEvent()
    {
        _crossCd.OnCrossTimerChange -= UpdateSliderCrossValue;
        _image.fillAmount = 1;
    }

    void UpdateSliderCrossValue()
    {
        _image.fillAmount = _crossCd.Cooldown / 30;
    }

    void UpdateSliderBibleValue()
    {
        _image.fillAmount = _bibleCd.Cooldown / 10;
    }


    public void SubscribeToBibleEvent()
    {
        _bibleCd = PlayerHandler.Instance.GetComponent<BibleCD>();
        _bibleCd.OnBibleTimerChange += UpdateSliderBibleValue;
    }

    public void UnSubscribeToBibleEvent()
    {
        _bibleCd.OnBibleTimerChange -= UpdateSliderBibleValue;
        _image.fillAmount = 1;
    }

}
