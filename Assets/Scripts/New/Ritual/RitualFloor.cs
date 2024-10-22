using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualFloor : MonoBehaviour, IInteractable
{
    private RitualManager _ritualManager;
    public bool canShowText;

    private void Start()
    {
        _ritualManager = RitualManager.Instance;
    }

    public void OnInteractItem()
    {
        if (!_ritualManager.candleTaked) return;
        _ritualManager.CheckCandleFloor();
    }

    private void Update()
    {
        if (Inventory.Instance.selectedItem == null)
        {
            canShowText = false;
            return;
        }
        canShowText = Inventory.Instance.selectedItem.itemName == "Candle";
    }

    public void OnInteract(bool hit, RaycastHit i)
    {

    }

    public void OnInteractWithObject()
    {
        
    }

    public string ShowText()
    {
        return !_ritualManager.candleTaked ? "" : "Place Candle";
    }

    public bool CanShowText()
    {
        return canShowText;
    }
}
