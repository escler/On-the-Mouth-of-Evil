using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerNew : MonoBehaviour
{
    public static GameManagerNew Instance { get; private set; }
    public bool cantPause;

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
        cantPause = true;
        if(PlayerHandler.Instance != null) PlayerHandler.Instance.UnPossesPlayer();
        print("Entre a cor");
        FadeOutHandler.Instance.FaceOut(1);
        yield return new WaitUntil(() => FadeOutHandler.Instance.fadeOut);
        FadeOutHandler.Instance.loadingScreen.SetActive(true);
        yield return new WaitForSeconds(2f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        FadeOutHandler.Instance.loadingScreen.SetActive(false);
        if(PlayerHandler.Instance != null) PlayerHandler.Instance.PossesPlayer();
        cantPause = false;
    }
}
