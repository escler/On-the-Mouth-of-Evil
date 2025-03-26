using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
    }

    public void ResetPrefs()
    {
        if (PlayerPrefs.HasKey("GoodPath")) PlayerPrefs.SetInt("GoodPath", 0);
        if (PlayerPrefs.HasKey("RespectAmount")) PlayerPrefs.SetInt("RespectAmount", 0);
        if (PlayerPrefs.HasKey("CurrencyAmount")) PlayerPrefs.SetInt("CurrencyAmount", 0);
        if (PlayerPrefs.HasKey("GoodEssencesAmount")) PlayerPrefs.SetInt("GoodEssencesAmount", 0);
        if (PlayerPrefs.HasKey("BadEssencesAmount")) PlayerPrefs.SetInt("BadEssencesAmount", 0);
        
        PlayerPrefs.Save();
    }
}
