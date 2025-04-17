using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleRitual : MonoBehaviour, IInteractable
{
    public bool isCandlePlaced;
    public Candle candle;

    public void GetCandle(Candle candlePlaced)
    {
        candle = candlePlaced;
        StartCoroutine(WaitForBool());
        Inventory.Instance.DropItem(Inventory.Instance.selectedItem, Inventory.Instance.countSelected);
        candle.transform.position = transform.position;
        candlePlaced.transform.rotation = transform.rotation;
        RitualManager.Instance.CandlePlaced(candle);
        candle.GetComponent<BoxCollider>().enabled = false;
        candle.GetComponent<Rigidbody>().isKinematic = true;
        candle.canShowText = true;
    }

    IEnumerator WaitForBool()
    {
        yield return new WaitForSeconds(0.5f);
        isCandlePlaced = true;
    }
    public void OnInteractItem()
    {
        if (!isCandlePlaced) return;
        isCandlePlaced = false;
        candle.OnGrabItem();
        RemoveCandle();
    }

    public void RemoveCandle()
    {
        RitualManager.Instance.RemoveCandle(candle);
        candle = null;
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public void OnInteractWithObject()
    {
    }

    public string ShowText()
    {
        return "";
    }

    public bool CanShowText()
    {
        return isCandlePlaced;
    }
}
