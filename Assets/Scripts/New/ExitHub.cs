using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitHub : MonoBehaviour, IInteractable
{
    public string interactDescription;
    private int _count;
    private int _itemNeeded = 4;
    private List<string> checkedItems = new List<string>();
    private AudioSource audioSource;
    public string soundName;

    private void Awake()
    {
        print(gameObject.name);
    }

    public void OnInteractItem()
    {
        _count = 0;
        var playerInventory = Inventory.Instance.hubInventory;
        if (PlayerHandler.Instance.actualMission == null) return;
        var actualMissionItemsNeededs = PlayerHandler.Instance.actualMission.
            itemsNeededs.Select(item => item.name);

        for (int i = 0; i < Inventory.Instance.capacity; i++)
        {
            if (playerInventory[i] == null) return;
            if (actualMissionItemsNeededs.Contains(playerInventory[i].itemName) && !checkedItems.Contains(playerInventory[i].itemName))
            {
                checkedItems.Add(playerInventory[i].itemName);
                _count++;
                continue;
            }

            break;
        }
        
        print(_count);

        if (_count < _itemNeeded)
        {
            checkedItems.Clear();
            return;
        }
        MusicManager.Instance.PlaySound(soundName, false);
        GameManagerNew.Instance.LoadSceneWithDelay(PlayerHandler.Instance.actualMission.misionName, 3f);
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public void OnInteractWithObject()
    {
        
    }

    public string ShowText()
    {
        return interactDescription;
    }

    public bool CanShowText()
    {
        return true;
    }
}
