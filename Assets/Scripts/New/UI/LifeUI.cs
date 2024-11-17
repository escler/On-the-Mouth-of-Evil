using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LifeUI : MonoBehaviour
{
    private PlayerLifeHandlerNew _lifeHandler;
    public Image lifeUI;
    public Sprite normalCoin, brokedCoin;
    private int actualLife;

    private void Awake()
    {
        StartCoroutine(WaitCor()); 
        EnableUI(SceneManager.GetActiveScene(),LoadSceneMode.Single);
    }

    IEnumerator WaitCor()
    {
        yield return new WaitForSeconds(0.1f);
        _lifeHandler = PlayerLifeHandlerNew.Instance;
        _lifeHandler.OnLifeChange += ChangeUI;
        actualLife = _lifeHandler.ActualLife;
        SceneManager.sceneLoaded += ResetUI;
        SceneManager.sceneLoaded += EnableUI;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= ResetUI;
        SceneManager.sceneLoaded -= EnableUI;
        if(_lifeHandler != null) _lifeHandler.OnLifeChange -= ChangeUI;
    }

    private void ChangeUI()
    {
        actualLife = _lifeHandler.ActualLife;
        
        lifeUI.sprite = brokedCoin;
    }

    private void ResetUI(Scene scene, LoadSceneMode loadSceneMode)
    {
        lifeUI.sprite = normalCoin;
    }

    private void EnableUI(Scene scene, LoadSceneMode loadSceneMode)
    {
        var inHubScene = scene.name == "Hub";

        lifeUI.enabled = !inHubScene;
    }
}
