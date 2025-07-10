using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfectionScreen : MonoBehaviour
{
    private Image _screenUI;
    [SerializeField] private Color normalColor, transparentColor;
    [SerializeField] private AnimationCurve initialCurve, loopCurve;
    [SerializeField] private float fadeOutDuration;
    private bool _coroutineActivate;
    IEnumerator _screenCouroutine;

    private void Awake()
    {
        _screenUI = GetComponent<Image>();
        StopLoopAndFadeOut();
        StartCoroutine(InitsParams());
        
    }

    private void OnDestroy()
    {
        InfectionHandler.Instance.OnUpdateInfection -= ChangeInfectionScreen;
    }

    IEnumerator InitsParams()
    {
        yield return null;
        InfectionHandler.Instance.OnUpdateInfection += ChangeInfectionScreen;
    }

    private void ChangeInfectionScreen()
    {
        var infectionRoom = PlayerHandler.Instance.actualRoom != null && PlayerHandler.Instance.actualRoom.swarmActivate;

        if (infectionRoom)
        {
            if (_screenCouroutine != null) return;
            _screenCouroutine = MakeScreenEffect();
            StartCoroutine(_screenCouroutine);
        }
        else
        {
            if (_coroutineActivate) return;
            StopLoopAndFadeOut();
        }
    }
    
    private void StopLoopAndFadeOut()
    {
        _coroutineActivate = true;
        if(_screenCouroutine != null) StopCoroutine(_screenCouroutine);
        StartCoroutine(FadeOutAlpha());
    }

    private IEnumerator FadeOutAlpha()
    {
        float startAlpha = _screenUI.color.a;
        float timer = 0f;

        while (timer < fadeOutDuration)
        {
            float t = timer / fadeOutDuration;
            float alpha = Mathf.Lerp(startAlpha, 0f, t);
            SetImageAlpha(alpha);

            timer += Time.deltaTime;
            yield return null;
        }
        
        _coroutineActivate = false;
        _screenCouroutine = null;
        SetImageAlpha(0f); // Asegura que quede en 0
    }

    private IEnumerator MakeScreenEffect()
    {
        // FASE 1: Fade-in usando fadeInCurve
        float timer = 0f;
        while (timer < 1)
        {
            float t = timer / 1;
            float alpha = initialCurve.Evaluate(t);
            SetImageAlpha(alpha);

            timer += Time.deltaTime;
            yield return null;
        }

        SetImageAlpha(initialCurve.Evaluate(1f)); // asegura el final exacto
        
        while (true)
        {
            timer = 0f;

            while (timer < 2)
            {
                float t = timer / 2;
                float alpha = loopCurve.Evaluate(t);

                SetImageAlpha(alpha);

                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
    
    private void SetImageAlpha(float alpha)
    {
        if (_screenUI != null)
        {
            Color c = _screenUI.color;
            c.a = Mathf.Clamp01(alpha);
            _screenUI.color = c;
        }
    }
}
