using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrencyHandler : MonoBehaviour
{
    private int _currentAmount;
    public int CurrentAmount => _currentAmount;
    public static CurrencyHandler Instance { get; private set; }

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
        if (PlayerPrefs.HasKey("CurrencyAmount"))
        {
            _currentAmount = PlayerPrefs.GetInt("CurrencyAmount");
            return;
        }
        
        PlayerPrefs.SetInt("CurrencyAmount", _currentAmount);
        PlayerPrefs.Save();
    }

    public void AddCurrency(int amount)
    {
        _currentAmount += amount;
        SaveCurrency();
        OnUpdateCurrency?.Invoke();
    }

    public void SubtractCurrency(int amount)
    {
        _currentAmount -= amount;
        SaveCurrency();
        OnUpdateCurrency?.Invoke();
    }

    void SaveCurrency()
    {
        PlayerPrefs.SetInt("CurrencyAmount", _currentAmount);
        PlayerPrefs.Save();
    }
}
