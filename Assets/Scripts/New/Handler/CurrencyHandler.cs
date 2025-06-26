using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrencyHandler : MonoBehaviour
{
    private int _currentAmount;
    public int CurrentAmount => _currentAmount;
    [SerializeField] private int initialAmount = 100;
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
        
        CheckPrefs();
        if (scene.name != "Menu") return;
        
        Destroy(gameObject);
    }
    private void CheckPrefs()
    {
        var tutorialComplete = PlayerPrefs.GetInt("TutorialCompleted", 0);
        if (PlayerPrefs.HasKey("CurrencyAmount") && tutorialComplete == 1)
        {
            _currentAmount = PlayerPrefs.GetInt("CurrencyAmount");
            return;
        }
        
        PlayerPrefs.SetInt("CurrencyAmount", initialAmount);
        _currentAmount = initialAmount;
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
