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
        if (PlayerPrefs.HasKey("SaltCount")) PlayerPrefs.SetInt("SaltCount", 0);
        if (PlayerPrefs.HasKey("CrossCount")) PlayerPrefs.SetInt("CrossCount", 0);
        if (PlayerPrefs.HasKey("LighterCount")) PlayerPrefs.SetInt("LighterCount", 0);
        if (PlayerPrefs.HasKey("BibleCount")) PlayerPrefs.SetInt("BibleCount", 0);
        if (PlayerPrefs.HasKey("RosaryCount")) PlayerPrefs.SetInt("RosaryCount", 0);
        if (PlayerPrefs.HasKey("VoodooCount")) PlayerPrefs.SetInt("VoodooCount", 0);
        
        
        PlayerPrefs.Save();
    }
}
