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
    [SerializeField] private ItemUIUnlocked goodItem, badItem;
    private void OnEnable()
    {
        print(PlayerPrefs.GetInt("RosaryAvaible") + " Rosario");
        print(PlayerPrefs.GetInt("VoodooAvaible") + " Voodoo");
        print(PlayerPrefs.GetInt("SwarmAvaible") + " Swarm");
        print(PlayerPrefs.GetInt("InciensoAvaible") + " Incienso");
        newItemUnlockedText.SetActive(false);
        CheckLevelsItems();
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
                    badItem.UnlockItem();
                    PlayerPrefs.SetInt("VoodooAvaible", 1);
                    PlayerPrefs.Save();
                    newItemUnlockedText.SetActive(true);
                    goodItem.UnlockedItem(PlayerPrefs.GetInt("RosaryAvaible") == 1);
                    return;
                }
                
                goodItem.UnlockedItem(PlayerPrefs.GetInt("RosaryAvaible") == 1);
                badItem.UnlockedItem(PlayerPrefs.GetInt("VoodooAvaible") == 1);
                
            }
            else
            {
                if (PlayerPrefs.GetInt("RosaryAvaible") == 0)
                {
                    goodItem.UnlockItem();
                    PlayerPrefs.SetInt("RosaryAvaible", 1);
                    PlayerPrefs.Save();
                    newItemUnlockedText.SetActive(true);
                    badItem.UnlockedItem(PlayerPrefs.GetInt("VoodooAvaible") == 1);
                    return;
                }

                print("Aca");
                goodItem.UnlockedItem(PlayerPrefs.GetInt("RosaryAvaible") == 1);
                badItem.UnlockedItem(PlayerPrefs.GetInt("VoodooAvaible") == 1);
            }
        }

        if (name != "MorgueLevel") return;
        
        if (badPath)
        {
            if (PlayerPrefs.GetInt("SwarmAvaible") == 0)
            {
                badItem.UnlockItem();
                PlayerPrefs.SetInt("SwarmAvaible", 1);
                PlayerPrefs.Save();
                newItemUnlockedText.SetActive(true);
                goodItem.UnlockedItem(PlayerPrefs.GetInt("InciensoAvaible") == 1);
                return;
            }
                    
            goodItem.UnlockedItem(PlayerPrefs.GetInt("InciensoAvaible") == 1);
            badItem.UnlockedItem(PlayerPrefs.GetInt("SwarmAvaible") == 1);
        }
        else
        {
            if (PlayerPrefs.GetInt("InciensoAvaible") == 0)
            {
                goodItem.UnlockItem();
                PlayerPrefs.SetInt("InciensoAvaible", 1);
                PlayerPrefs.Save();
                newItemUnlockedText.SetActive(true);
                badItem.UnlockedItem(PlayerPrefs.GetInt("SwarmAvaible") == 1);
                return;
            }
                    
            goodItem.UnlockedItem(PlayerPrefs.GetInt("InciensoAvaible") == 1);
            badItem.UnlockedItem(PlayerPrefs.GetInt("SwarmAvaible") == 1);
        }
    }
}
