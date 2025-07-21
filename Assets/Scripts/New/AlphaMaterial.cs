using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaMaterial : MonoBehaviour
{
    public Material mat;
    public float fadeDuration = 2f;
    IEnumerator _fadeInCor, _fadeOutCor;

    void Awake()
    {
        mat = new Material(mat);
        GetComponent<Renderer>().material = mat;

        mat.SetFloat("_Alpha", 0f);
    }

    public void FadeIn()
    {
        if (_fadeOutCor != null)
        {
            StopCoroutine(_fadeOutCor);
            _fadeOutCor = null;
        }

        if (_fadeInCor != null) return;

        _fadeInCor = FadeInCor();
        StartCoroutine(_fadeInCor);
    }

    public void FadeOut()
    {
        if (_fadeInCor != null)
        {
            StopCoroutine(_fadeInCor);
            _fadeInCor = null;
        }

        if (_fadeOutCor != null) return;

        _fadeOutCor = FadeOutCor();
        StartCoroutine(_fadeOutCor);
    }

    IEnumerator FadeInCor()
    {
        float startAlpha = mat.GetFloat("_Alpha");
        float targetAlpha = 1f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(targetAlpha);
        _fadeInCor = null;
    }

    IEnumerator FadeOutCor()
    {
        float startAlpha = mat.GetFloat("_Alpha");
        float targetAlpha = 0f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(targetAlpha);
        _fadeOutCor = null;
    }

    void SetAlpha(float alpha)
    {
        mat.SetFloat("_Alpha", alpha);
    }
}