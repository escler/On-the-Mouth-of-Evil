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

    public void OnInteractItem()
    {
        _count = 0;
        var playerInventory = Inventory.Instance.hubInventory;
        if (PlayerHandler.Instance.actualMission == null) return;
        var actualMissionItemsNeededs = PlayerHandler.Instance.actualMission.
            itemsNeededs.Select(item => item.name);

        for (int i = 0; i < playerInventory.Length; i++)
        {
            if (actualMissionItemsNeededs.Contains(playerInventory[i].itemName))
            {
                _count++;
                continue;
            }

            break;
        }
        
        if (_count != _itemNeeded) return;

        SceneManager.LoadScene(PlayerHandler.Instance.actualMission.misionName);

    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public string ShowText()
    {
        return interactDescription;
    }
}
