using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerNew : MonoBehaviour
{
    public static GameManagerNew Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);

        Instance = this;
    }

    public void LoadSceneWithDelay(string sceneName, float seconds)
    {
        StartCoroutine(LoadSceneCor(sceneName, seconds));
    }

    IEnumerator LoadSceneCor(string sceneName, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(sceneName);
    }
}
