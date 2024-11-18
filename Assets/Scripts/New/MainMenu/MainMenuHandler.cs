using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    public Button playBTN, quitBTN;

    private void Awake()
    {
        playBTN.onClick.AddListener(LoadHubScene);
        quitBTN.onClick.AddListener(QuitGame);
    }

    private void OnDestroy()
    {
        playBTN.onClick.RemoveAllListeners();
        quitBTN.onClick.RemoveAllListeners();
    }

    private void LoadHubScene()
    {
        GameManagerNew.Instance.LoadSceneWithDelay("Hub", 5f);
    }

    private void QuitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }
}
