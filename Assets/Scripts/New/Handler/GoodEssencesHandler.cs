using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodEssencesHandler : MonoBehaviour
{
    private int _currentAmount;
    public static GoodEssencesHandler Instance { get; private set; }

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
    }

    private void CheckPrefs()
    {
        if (PlayerPrefs.HasKey("GoodEssencesAmount"))
        {
            _currentAmount = PlayerPrefs.GetInt("GoodEssencesAmount");
            return;
        }
        
        PlayerPrefs.SetInt("GoodEssencesAmount", _currentAmount);
        PlayerPrefs.Save();
    }

    private void AddCurrency(int amount)
    {
        _currentAmount += amount;
        SaveEssences();
    }

    private void SubtractCurrency(int amount)
    {
        _currentAmount -= amount;
        SaveEssences();
    }

    void SaveEssences()
    {
        PlayerPrefs.SetInt("GoodEssencesAmount", _currentAmount);
        PlayerPrefs.Save();
    }
}
