using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FadeOutHandler : MonoBehaviour
{
    public static FadeOutHandler Instance { get; private set; }
    private Image _image;
    private float interval;
    private Color imageColor;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _image = GetComponent<Image>();
        imageColor = _image.color;
        SceneManager.sceneLoaded += ResetAlpha;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= ResetAlpha;
    }

    public void FaceOut(float duration)
    {
        StartCoroutine(FadeOutCor(duration));
    }

    IEnumerator FadeOutCor(float duration)
    {
        interval = (1 - _image.color.a) / duration;
        while (_image.color.a < 1)
        {
            print("Entre aca");
            imageColor.a += interval * 0.01f;
            print(_image.color.a);
            _image.color = imageColor;
            yield return new WaitForSeconds(0.01f);
        }

        imageColor.a = 1;
        _image.color = imageColor;
    }

    private void ResetAlpha(Scene scene, LoadSceneMode loadSceneMode)
    {
        StopAllCoroutines();
        imageColor.a = 0;
        _image.color = imageColor;
    }

    IEnumerator FadeIn(float duration)
    {
        interval = imageColor.a / duration;
        while (_image.color.a > 0)
        {
            imageColor.a -= interval * 0.1f;
            _image.color = imageColor;
            yield return new WaitForSeconds(0.1f);
        }

        imageColor.a = 0;
        _image.color = imageColor;
    }
}
