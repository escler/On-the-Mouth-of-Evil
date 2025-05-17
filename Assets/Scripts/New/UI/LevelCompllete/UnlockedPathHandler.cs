using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UnlockedPathHandler : MonoBehaviour
{
    [SerializeField] private GameObject newItemUnlockedText;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private GoodItemUnlocked goodItem, badItem;
    private void OnEnable()
    {
        CheckLevelsItems();
        newItemUnlockedText.SetActive(false);
        title.text = DecisionsHandler.Instance.badPath ? "Bad Ending Done" : "Good Ending Done";
    }

    private void OnDisable()
    {
    }

    void CheckLevelsItems()
    {
        string level = SceneManager.GetActiveScene().name;
        var badPath = DecisionsHandler.Instance.badPath;
        if (level == "HouseLevel")
        {
            if (badPath)
            {
                if (PlayerPrefs.GetInt("VoodooAvaible") == 0)
                {
                    PlayerPrefs.SetInt("VoodooAvaible", 1);
                    newItemUnlockedText.SetActive(true);
                    badItem.DoAnimation();
                    return;
                } 
            }
            else
            {
                if (PlayerPrefs.GetInt("RosaryAvaible") == 0)
                {
                    PlayerPrefs.SetInt("RosaryAvaible", 1);
                    newItemUnlockedText.SetActive(true);
                    goodItem.DoAnimation();
                    return;
                }
            }
        }

        if (badPath)
        {
            if (PlayerPrefs.GetInt("SwarmAvaible") == 0)
            {
                PlayerPrefs.SetInt("SwarmAvaible", 1);
                newItemUnlockedText.SetActive(true);
                badItem.DoAnimation();
                return;
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("InciensoAvaible") == 0)
            {
                PlayerPrefs.SetInt("InciensoAvaible", 1);
                newItemUnlockedText.SetActive(true);
                goodItem.DoAnimation();
            }
        }
        
    }
}
