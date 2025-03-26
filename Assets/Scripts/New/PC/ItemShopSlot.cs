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

    
    private void OnEnable()
    {
        purchaseBTN.interactable = unlocked;
        CurrencyHandler.Instance.OnUpdateCurrency += CheckColor;
        CurrencyHandler.Instance.OnUpdateCurrency += CheckInteractable;
        ShowCost();
        CheckColor();
        CheckInteractable();
        purchaseBTN.onClick.AddListener(BuyItem);
    }

    private void OnDisable()
    {
        CurrencyHandler.Instance.OnUpdateCurrency -= CheckColor;
        CurrencyHandler.Instance.OnUpdateCurrency -= CheckInteractable;
        purchaseBTN.onClick.RemoveAllListeners();
    }

    private void CheckInteractable()
    {
        purchaseBTN.interactable = CanInteract();
    }

    private bool CanInteract()
    {
        //Max Stock return false
        if (cost > CurrencyHandler.Instance.CurrentAmount) return false;
        return true;
    }

    private void ShowCost()
    {
        if (!unlocked) return;
        purchaseBTN.GetComponentInChildren<TextMeshProUGUI>().text = cost.ToString();
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
        //Dar el item
    }
}
