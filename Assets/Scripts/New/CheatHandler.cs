using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] missions;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) UnlockMissions(); //Desbloquear Misiones
        if (Input.GetKeyDown(KeyCode.F2)) ShowButtonsPC(); // Currency buttons
        if (Input.GetKeyDown(KeyCode.F3)) UnlockItems(); // Currency buttons
    }

    private void UnlockItems()
    {
        PlayerPrefs.SetInt("BadPath", 1);
        PlayerPrefs.Save();
        PCHandler.Instance.CheckBadShopEnable();
        PCHandler.Instance.badShopApp.interactable = true;
        PlayerPrefs.SetInt("VoodooAvaible", 1);
        PlayerPrefs.SetInt("RosaryAvaible", 1);
        PlayerPrefs.SetInt("IncenseAvaible", 1);
        PlayerPrefs.SetInt("SwarmAvaible", 1);
        
        PlayerPrefs.SetInt("VoodooUnlocked", 1);
        PlayerPrefs.SetInt("RosaryUnlocked", 1);
        PlayerPrefs.SetInt("IncenseUnlocked", 1);
        PlayerPrefs.SetInt("SwarmUnlocked", 1);
        
        PlayerPrefs.Save();
    }

    private void ShowButtonsPC()
    {
        PCHandler.Instance.currencyButtons.SetActive(true);
    }

    void UnlockMissions()
    {
        foreach (var m in missions)
        {
            m.SetActive(true);
        }
    }
}
