using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerNew : MonoBehaviour
{
    public static GameManagerNew Instance { get; private set; }
    public bool cantPause, canProceed;

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

    public void LoadCurrencyStats(string sceneName, float seconds)
    {
        StartCoroutine(LoadCompleteLevel(sceneName, seconds));
    }

    IEnumerator LoadCompleteLevel(string sceneName, float seconds)
    {
        cantPause = true;
        CheckLevelAndSave();
        if(PlayerHandler.Instance != null) PlayerHandler.Instance.UnPossesPlayer();
        print("Entre a cor");
        FadeOutHandler.Instance.FaceOut(1);
        
        yield return new WaitUntil(() => FadeOutHandler.Instance.fadeOut);

        FadeOutHandler.Instance.currency.SetActive(true);
        canProceed = false;
        
        yield return new WaitUntil(() => canProceed);
        FadeOutHandler.Instance.proceedButton.SetActive(true);
        
        yield return new WaitUntil(() => Input.anyKeyDown);
        FadeOutHandler.Instance.proceedButton.SetActive(false);
        FadeOutHandler.Instance.currency.SetActive(false);
        FadeOutHandler.Instance.loadingScreen.SetActive(true);
        yield return new WaitForSeconds(2f);

        bool badPath = DecisionsHandler.Instance.badPath;

        if (!badPath)
        {
            int goodMission1 = PlayerPrefs.GetInt("RosaryUnlocked");
            int goodMission2 = PlayerPrefs.GetInt("IncenseUnlocked");
            if (goodMission1 == 1 && goodMission2 == 1)
            {
                if (PlayerPrefs.GetInt("EndingGood") == 0)
                {
                    FadeOutHandler.Instance.loadingScreen.SetActive(false);
                    FadeOutHandler.Instance.ShowEndingGood();
                    yield break;
                }
            }
        }
        else
        {
            int badMission1 = PlayerPrefs.GetInt("VoodooUnlocked");
            int badMission2 = PlayerPrefs.GetInt("SwarmUnlocked");
            if (badMission1 == 1 && badMission2 == 1)
            {
                if (PlayerPrefs.GetInt("EndingBad") == 0)
                {
                    FadeOutHandler.Instance.loadingScreen.SetActive(false);
                    FadeOutHandler.Instance.ShowEndingBad();
                    yield break;
                }
            }
        }

        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        FadeOutHandler.Instance.loadingScreen.SetActive(false);
        if(PlayerHandler.Instance != null) PlayerHandler.Instance.PossesPlayer();
        cantPause = false;
    }

    private void CheckLevelAndSave()
    {
        var nameScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt(nameScene == "HouseLevel" ? "Mission1Complete" : "Mission2Complete", 1);
    }

    public void CheckProceed()
    {
        var currencies = FadeOutHandler.Instance.currency.GetComponentsInChildren<CurrencyUIGained>();
        int count = 0;

        foreach (var c in currencies)
        {
            if (!c.IsDone) break;
            count++;
        }
        
        canProceed = count == currencies.Length;
    }
}
