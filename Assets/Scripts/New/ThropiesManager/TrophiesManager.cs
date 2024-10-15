using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrophiesManager : MonoBehaviour
{
    public static TrophiesManager Instance { get; private set; }
    private int _goodPath, _badPath;

    public int GoodPath => _goodPath;
    public int BadPath => _badPath;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
        SearchPrefs();
        
        SceneManager.sceneLoaded += CheckThropies;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= CheckThropies;
    }

    void SearchPrefs()
    {
        _goodPath = PlayerPrefs.HasKey("GoodPath") ? PlayerPrefs.GetInt("GoodPath") : 0;
        _badPath = PlayerPrefs.HasKey("BadPath") ? PlayerPrefs.GetInt("BadPath") : 0;
    }
    
    private void CheckThropies(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (SceneManager.GetActiveScene().name != "Hub") return;

        StartCoroutine(CheckThropiesCor());
    }

    IEnumerator CheckThropiesCor()
    {
        yield return new WaitForSeconds(.1f);
        if (TrophiesHandler.Instance == null) yield break;
        var goodObjects = TrophiesHandler.Instance.goodThropies;

        for (int i = 0; i < _goodPath; i++)
        {
            if (i >= goodObjects.Length) break;
            goodObjects[i].SetActive(true);
        }

        var badObjects = TrophiesHandler.Instance.badThropies;

        for (int i = 0; i < _badPath; i++)
        {
            if (i >= badObjects.Length) break;
            badObjects[i].SetActive(true);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O)) ResetPrefs();
    }

    void ResetPrefs()
    {
        if (PlayerPrefs.HasKey("GoodPath")) PlayerPrefs.SetInt("GoodPath", 0);
        if (PlayerPrefs.HasKey("BadPath")) PlayerPrefs.SetInt("BadPath", 0);
        PlayerPrefs.Save();
    }

    public void ChangePrefs(string pref)
    {
        switch (pref)
        {
            case "GoodPath":
                _goodPath++;
                PlayerPrefs.SetInt(pref,_goodPath);
                break;
            case "BadPath":
                _badPath++;
                PlayerPrefs.SetInt(pref, _badPath);
                break;
        }
        
        PlayerPrefs.Save();
    }
}
