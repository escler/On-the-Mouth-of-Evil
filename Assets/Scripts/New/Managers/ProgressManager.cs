using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance { get; private set; }
    public GameObject[] missions;

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
        PlayerPrefs.SetInt("GoodPath", 0);
        PlayerPrefs.SetInt("BadPath", 0);
        PlayerPrefs.SetInt("RespectAmount", 0);
        PlayerPrefs.SetInt("RespectLevel", 1);
        PlayerPrefs.SetInt("CurrencyAmount", 100);
        PlayerPrefs.SetInt("GoodEssencesAmount", 0);
        PlayerPrefs.SetInt("BadEssencesAmount", 0);
        PlayerPrefs.SetInt("SaltCount", 0);
        PlayerPrefs.SetInt("CrossCount", 0);
        PlayerPrefs.SetInt("LighterCount", 0);
        PlayerPrefs.SetInt("BibleCount", 0);
        PlayerPrefs.SetInt("RosaryCount", 0);
        PlayerPrefs.SetInt("VoodooCount", 0);
        PlayerPrefs.SetInt("VoodooUnlocked", 0);
        PlayerPrefs.SetInt("RosaryUnlocked", 0);
        PlayerPrefs.SetInt("InciensoUnlocked", 0);
        PlayerPrefs.SetInt("SwarmUnlocked", 0);
        PlayerPrefs.SetInt("Mission1Complete", 0);
        PlayerPrefs.SetInt("Mission2Complete", 0);
        PlayerPrefs.SetInt("TutorialCompleted", 0); 
        PlayerPrefs.SetInt("VoodooAvaible", 0);
        PlayerPrefs.SetInt("RosaryAvaible", 0);
        PlayerPrefs.SetInt("IncenseAvaible", 0);
        PlayerPrefs.SetInt("SwarmAvaible", 0);
        
        
        PlayerPrefs.Save();
    }
}
