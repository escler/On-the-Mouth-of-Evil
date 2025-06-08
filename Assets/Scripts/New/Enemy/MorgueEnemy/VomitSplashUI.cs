using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VomitSplashUI : MonoBehaviour
{
    public static VomitSplashUI Instance {get; private set;}
    private Image _splash;
    private float _actualTime;
    private bool _corroutineActive;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _splash = GetComponent<Image>();
        HideSplash();
        SceneManager.sceneLoaded += InitsParams;
    }

    private void Update()
    {
        if (_actualTime < 0) return;
        
        _actualTime -= Time.deltaTime;
        
    }

    public void AddTime(float time)
    {
        StopAllCoroutines();
        _corroutineActive = false;
        _actualTime = time;
        ShowSplash();
    }
    void ShowSplash()
    {
        var color = _splash.color;
        color.a = 1;
        _splash.color = color;
        if (_corroutineActive) return;
        _corroutineActive = true;
        StartCoroutine(HideLerpSplash());
    }

    IEnumerator HideLerpSplash()
    {
        _corroutineActive = true;
        yield return new WaitUntil(() => _actualTime <= 1);
        float time = 0;
        float alpha = _splash.color.a;
        while (time < 1)
        {
            time += Time.deltaTime;
            var color = _splash.color;
            color.a = Mathf.Lerp(alpha, 0, time);
            _splash.color = color;
            yield return null;
        }
    }
    void HideSplash()
    {
        var color = _splash.color;
        color.a = 0;
        _splash.color = color;
    }

    private void OnDestroy()
    {
        if (Instance != this) return;
        SceneManager.sceneLoaded -= InitsParams;
    }

    void InitsParams(Scene scene, LoadSceneMode loadSceneMode)
    {
        var color = _splash.color;
        color.a = 0;
        _splash.color = color;

    }
}
