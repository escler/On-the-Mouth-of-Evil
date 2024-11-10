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
    public Color flashColor;
    public float interval;

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
        if (_image.fillAmount < 1) return;
        DoFlash();
    }

    void UpdateSliderBibleValue()
    {
        _image.fillAmount = _bibleCd.Cooldown / 10;
        if (_image.fillAmount < 1) return;
        DoFlash();
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

    public void DoFlash()
    {
        StartCoroutine(DoFlashCor());
    }

    IEnumerator DoFlashCor()
    {
        var flashImage = transform.GetChild(0).GetComponent<Image>();
        print(flashImage.gameObject.name);
        flashColor.a = 1;
        flashImage.color = flashColor;
        while (flashColor.a > 0)
        {
            flashColor.a -= interval;
            flashImage.color = flashColor;
            yield return new WaitForSeconds(0.02f);
        }

        flashColor.a = 0;
        flashImage.color = flashColor;
    }
}
