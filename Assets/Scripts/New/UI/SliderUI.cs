using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SliderUI : MonoBehaviour
{
    private Image _image;
    private CrossCD _crossCd;
    private BibleCD _bibleCd;
    private bool crossSubscriber, bibleSubscriber;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void SubscribeToCrossEvent()
    {
        _crossCd = PlayerHandler.Instance.GetComponent<CrossCD>();
        _crossCd.OnCrossTimerChange += UpdateSliderCrossValue;
        crossSubscriber = true;
    }

    public void UnSubscribeToCrossEvent()
    {
        if (!crossSubscriber) return;
        _crossCd.OnCrossTimerChange -= UpdateSliderCrossValue;
        _image.fillAmount = 1;
        crossSubscriber = false;
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
        bibleSubscriber = true;
    }

    public void UnSubscribeToBibleEvent()
    {
        if (!bibleSubscriber) return;
        _bibleCd.OnBibleTimerChange -= UpdateSliderBibleValue;
        _image.fillAmount = 1;
        bibleSubscriber = false;
    }

    public void ClearSubscripcion()
    {
        UnSubscribeToBibleEvent();
        UnSubscribeToCrossEvent();
    }
}
