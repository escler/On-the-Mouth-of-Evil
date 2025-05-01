using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespectHandler : MonoBehaviour
{
    [SerializeField] Dictionary<int, int[]> levels = new Dictionary<int, int[]>();
    public Dictionary<int, int[]> Levels => levels;
    private int _currentAmount;
    public int CurrentAmount => _currentAmount;
    
    private int _currentLevel;
    public int CurrentLevel => _currentLevel;
    public static RespectHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        levels.Add(1, new[] { 0, 50 });
        levels.Add(2, new[] { 50, 150 });
        levels.Add(3, new[] { 150, 300 });
        levels.Add(4, new[] { 300, 600 });
        levels.Add(5, new[] { 600, 1200 });
        DontDestroyOnLoad(this);
        CheckPrefs();
        CheckLevelPrefs();
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

    private void CheckLevelPrefs()
    {
        if (PlayerPrefs.HasKey("RespectLevel"))
        {
            _currentLevel = PlayerPrefs.GetInt("RespectLevel");
            return;
        }

        PlayerPrefs.SetInt("RespectLevel", 1);
        _currentLevel = 1;
        PlayerPrefs.Save();
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

    public void AddRespect(int amount)
    {
        _currentAmount += amount;
        SaveRespect();
    }

    public void AddLevel(int amount)
    {
        _currentLevel += amount;
        SaveRespect();
    }

    void SaveRespect()
    {
        PlayerPrefs.SetInt("RespectAmount", _currentAmount);
        PlayerPrefs.SetInt("RespectLevel", _currentLevel);
        PlayerPrefs.Save();
    }
}
