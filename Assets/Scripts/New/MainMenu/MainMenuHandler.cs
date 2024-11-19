using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    public Button playBTN, quitBTN, optionsBTN, backOptionsBTN;
    public GameObject mainButtons, options;

    private void Awake()
    {
        playBTN.onClick.AddListener(LoadHubScene);
        quitBTN.onClick.AddListener(QuitGame);
        optionsBTN.onClick.AddListener(OpenOptions);
        backOptionsBTN.onClick.AddListener(CloseOptions);
    }

    private void OnDestroy()
    {
        playBTN.onClick.RemoveAllListeners();
        quitBTN.onClick.RemoveAllListeners();
        optionsBTN.onClick.RemoveAllListeners();
    }

    private void LoadHubScene()
    {
        GameManagerNew.Instance.LoadSceneWithDelay("Hub", 5f);
    }

    private void OpenOptions()
    {
        mainButtons.SetActive(false);
        options.SetActive(true);
    }

    private void CloseOptions()
    {
        options.SetActive(false);
        mainButtons.SetActive(true);
    }

    private void QuitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }
}
