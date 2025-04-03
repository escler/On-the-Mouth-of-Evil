using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BadEssencesHandler : MonoBehaviour
{
    private int _currentAmount;
    public int CurrentAmount => _currentAmount;
    public static BadEssencesHandler Instance { get; private set; }
    
    public delegate void UpdateCurrency();

    public event UpdateCurrency OnUpdateCurrency;

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
        if (PlayerPrefs.HasKey("BadEssencesAmount"))
        {
            _currentAmount = PlayerPrefs.GetInt("BadEssencesAmount");
            return;
        }
        
        PlayerPrefs.SetInt("BadEssencesAmount", _currentAmount);
        PlayerPrefs.Save();
    }

    public void AddCurrency(int amount)
    {
        _currentAmount += amount;
        SaveEssences();
    }

    public void SubtractCurrency(int amount)
    {
        _currentAmount -= amount;
        SaveEssences();
    }

    void SaveEssences()
    {
        PlayerPrefs.SetInt("BadEssencesAmount", _currentAmount);
        PlayerPrefs.Save();
        OnUpdateCurrency?.Invoke();
    }
}
