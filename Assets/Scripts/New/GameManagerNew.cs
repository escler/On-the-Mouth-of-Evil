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
        PlayerHandler.Instance.UnPossesPlayer();
        print("Entre a cor");
        FadeOutHandler.Instance.FaceOut(1);
        yield return new WaitUntil(() => FadeOutHandler.Instance.fadeOut);
        CanvasManager.Instance.loadingScreen.SetActive(true);
        yield return new WaitForSeconds(2f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        CanvasManager.Instance.loadingScreen.SetActive(false);
        PlayerHandler.Instance.PossesPlayer();
    }
}
