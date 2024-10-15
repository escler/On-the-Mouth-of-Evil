using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LifeUI : MonoBehaviour
{
    private PlayerLifeHandlerNew _lifeHandler;
    public Image[] lifeUI;
    public Sprite normalCoin, brokedCoin;
    private int actualLife;

    private void Awake()
    {
        StartCoroutine(WaitCor());
    }

    IEnumerator WaitCor()
    {
        yield return new WaitForSeconds(0.1f);
        _lifeHandler = PlayerLifeHandlerNew.Instance;
        _lifeHandler.OnLifeChange += ChangeUI;
        actualLife = _lifeHandler.ActualLife;
        SceneManager.sceneLoaded += ResetUI;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= ResetUI;
        if(_lifeHandler != null) _lifeHandler.OnLifeChange -= ChangeUI;
    }

    private void ChangeUI()
    {
        actualLife = _lifeHandler.ActualLife;

        for (int i = 0; i < _lifeHandler.startLife - actualLife; i++)
        {
            lifeUI[i].sprite = brokedCoin;
        }
    }

    private void ResetUI(Scene scene, LoadSceneMode loadSceneMode)
    {
        for (int i = 0; i < lifeUI.Length; i++)
        {
            lifeUI[i].sprite = normalCoin;
        }
    }
}
