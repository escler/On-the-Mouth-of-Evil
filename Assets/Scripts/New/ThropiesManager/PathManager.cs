using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PathManager : MonoBehaviour
{
    public static PathManager Instance { get; private set; }
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
        var goodPathObjs = ItemPathHandler.Instance.goodPathObjs;


        for (int i = 0; i < _goodPath; i++)
        {
            if (i < goodObjects.Length)
                goodObjects[i].SetActive(true);
            
            if (i < goodPathObjs.Length)
                goodPathObjs[i].SetActive(true);
        }

        var badObjects = TrophiesHandler.Instance.badThropies;
        var badPathObjs = ItemPathHandler.Instance.badPathObjs;

        for (int i = 0; i < _badPath; i++)
        {
            if (i < badObjects.Length)
                badObjects[i].SetActive(true);

            if (i < badPathObjs.Length)
                badPathObjs[i].SetActive(true);
        }
    }

    public void ChangePrefs(string pref, int amount)
    {
        switch (pref)
        {
            case "GoodPath":
                PlayerPrefs.SetInt(pref,amount);
                break;
            case "BadPath":
                PlayerPrefs.SetInt(pref, amount);
                break;
        }
        
        PlayerPrefs.Save();
    }
}
