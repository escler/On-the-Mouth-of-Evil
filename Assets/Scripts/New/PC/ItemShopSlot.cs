using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemShopSlot : MonoBehaviour
{
    [SerializeField] private Item _item;
    [SerializeField] private int cost;
    public bool unlocked;
    [SerializeField] private Button purchaseBTN;
    [SerializeField] private Color canBuy, cantBuy;

    private InventoryItemHandler handler;

    
    private void OnEnable()
    {
        if (handler == null) handler = SortInventoryBuyHandler.Instance.GetHandler(_item);
        purchaseBTN.interactable = unlocked;
        CurrencyHandler.Instance.OnUpdateCurrency += CheckColor;
        CurrencyHandler.Instance.OnUpdateCurrency += CheckInteractable;
        CurrencyHandler.Instance.OnUpdateCurrency += ShowText;
        ShowText();
        CheckColor();
        CheckInteractable();
        purchaseBTN.onClick.AddListener(BuyItem);
    }

    private void OnDisable()
    {
        CurrencyHandler.Instance.OnUpdateCurrency -= CheckColor;
        CurrencyHandler.Instance.OnUpdateCurrency -= CheckInteractable;
        CurrencyHandler.Instance.OnUpdateCurrency -= ShowText;
        purchaseBTN.onClick.RemoveAllListeners();
    }
    private void CheckInteractable()
    {
        purchaseBTN.interactable = CanInteract();
    }

    private bool CanInteract()
    {
        if(handler.Count >= handler.countMax - 1) return false;
        if (cost > CurrencyHandler.Instance.CurrentAmount) return false;
        return true;
    }

    private void ShowText()
    {
        if (!unlocked) return;
        purchaseBTN.GetComponentInChildren<TextMeshProUGUI>().text = 
            handler.Count >= handler.countMax - 1 ? "Out of Stock" : cost.ToString();
    }

    private void CheckColor()
    {
        if (!unlocked) return;
        purchaseBTN.GetComponent<Image>().color = 
            cost <= CurrencyHandler.Instance.CurrentAmount ? canBuy : cantBuy;
    }

    private void BuyItem()
    {
        CurrencyHandler.Instance.SubtractCurrency(cost);
        SortInventoryBuyHandler.Instance.AddItemToHandler(_item);
    }
}
