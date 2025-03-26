using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadEssencesAmount : MonoBehaviour
{
    private int _currentAmount;
    public static BadEssencesAmount Instance { get; private set; }

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
        if (PlayerPrefs.HasKey("BadEssencesAmount"))
        {
            _currentAmount = PlayerPrefs.GetInt("BadEssencesAmount");
            return;
        }
        
        PlayerPrefs.SetInt("BadEssencesAmount", _currentAmount);
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
        PlayerPrefs.SetInt("BadEssencesAmount", _currentAmount);
        PlayerPrefs.Save();
    }
}
