using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespectHandler : MonoBehaviour
{
    private int _currentAmount;
    public static RespectHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        CheckPrefs();
        SceneManager.sceneLoaded += DestroyInMenu;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= DestroyInMenu;

    }

    private void DestroyInMenu(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name != "Menu") return;
        
        Destroy(gameObject);
    }
    private void CheckPrefs()
    {
        if (PlayerPrefs.HasKey("RespectAmount"))
        {
            _currentAmount = PlayerPrefs.GetInt("RespectAmount");
            return;
        }
        
        PlayerPrefs.SetInt("RespectAmount", _currentAmount);
        PlayerPrefs.Save();
    }

    private void AddRespect(int amount)
    {
        _currentAmount += amount;
        SaveRespect();
    }

    void SaveRespect()
    {
        PlayerPrefs.SetInt("RespectAmount", _currentAmount);
        PlayerPrefs.Save();
    }
}
