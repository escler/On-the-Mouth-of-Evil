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
    private Animator _animator;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        _animator = GetComponent<Animator>();
        Instance = this;
        _splash = GetComponentInChildren<Image>();
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

        if (_corroutineActive) return;
        _animator.SetBool("Start", true);
        _corroutineActive = true;
        StartCoroutine(HideLerpSplash());
    }


    IEnumerator HideLerpSplash()
    {
        _corroutineActive = true;
        yield return new WaitUntil(() => _actualTime <= 1);
        _animator.SetBool("Start", false);
    }
    public void HideSplash()
    {
        _splash.enabled = false;
    }

    private void OnDestroy()
    {
        if (Instance != this) return;
        SceneManager.sceneLoaded -= InitsParams;
    }

    void InitsParams(Scene scene, LoadSceneMode loadSceneMode)
    {
        StopAllCoroutines();
        _corroutineActive = false;
        _splash.enabled = false;
        _animator.SetBool("Start", false);
    }
}
