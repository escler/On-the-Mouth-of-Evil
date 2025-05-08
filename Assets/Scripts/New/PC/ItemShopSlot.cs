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
    [SerializeField] private int cost, costToUnlock;
    public bool unlocked;
    [SerializeField] private Button purchaseBTN, unlockBTN;
    [SerializeField] private Color canBuy, cantBuy;
    [SerializeField] private bool badItem;
    [SerializeField] private Image iconCurrency;
    [SerializeField] private Sprite goodCurrency, badCurrency;

    InventoryItemHandler _handler;
    public string item;

    
    private void OnEnable()
    {
        if (_handler == null) _handler = SortInventoryBuyHandler.Instance.GetHandler(_item);
        CheckPrefs();
        LockSettings();
        CurrencyHandler.Instance.OnUpdateCurrency += CheckColor;
        CurrencyHandler.Instance.OnUpdateCurrency += CheckInteractable;
        CurrencyHandler.Instance.OnUpdateCurrency += ShowText;
        _handler.OnUpdateCount += CheckColor;
        _handler.OnUpdateCount += CheckInteractable;
        _handler.OnUpdateCount += ShowText;
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
        _handler.OnUpdateCount -= CheckColor;
        _handler.OnUpdateCount -= CheckInteractable;
        _handler.OnUpdateCount -= ShowText;
        purchaseBTN.onClick.RemoveAllListeners();
    }

    private void OnDestroy()
    {
        CurrencyHandler.Instance.OnUpdateCurrency -= CheckColor;
        CurrencyHandler.Instance.OnUpdateCurrency -= CheckInteractable;
        CurrencyHandler.Instance.OnUpdateCurrency -= ShowText;
        _handler.OnUpdateCount -= CheckColor;
        _handler.OnUpdateCount -= CheckInteractable;
        _handler.OnUpdateCount -= ShowText;
        purchaseBTN.onClick.RemoveAllListeners();
    }

    private void CheckInteractable()
    {
        purchaseBTN.enabled = CanInteract();
    }

    private bool CanInteract()
    {
        if(!unlocked) return false;
        if (!TutorialHub.Instance.TutorialCompleted)
        {
            if(_handler.Count >= 1) return false;
        }
        if(_handler.Count >= _handler.countMax) return false;
        if (cost > CurrencyHandler.Instance.CurrentAmount) return false;
        return true;
    }

    private void ShowText()
    {
        if (!unlocked) return;
        
        if (!TutorialHub.Instance.TutorialCompleted)
        {
            purchaseBTN.GetComponentInChildren<TextMeshProUGUI>().text = 
                _handler.Count > 0 ? "Out of Stock" : cost.ToString();
            return;
        }
        purchaseBTN.GetComponentInChildren<TextMeshProUGUI>().text = 
            _handler.Count >= _handler.countMax ? "Out of Stock" : cost.ToString();

        purchaseBTN.GetComponentInChildren<TextMeshProUGUI>().fontSize =
            _handler.Count >= _handler.countMax ? 3f : 4;
    }

    private void CheckColor()
    {
        if (!unlocked) return;
        if (!TutorialHub.Instance.TutorialCompleted)
        {
            purchaseBTN.GetComponent<Image>().color = _handler.Count < 1 ? canBuy : cantBuy;
            TutorialHub.Instance.CheckStoreBuy();
            purchaseBTN.GetComponent<Image>().color = cost <= CurrencyHandler.Instance.CurrentAmount ? canBuy : cantBuy;
            return;
        }
        purchaseBTN.GetComponent<Image>().color = _handler.Count < _handler.countMax ? canBuy : cantBuy;
        
        if (_handler.Count >= _handler.countMax) return;
        
        purchaseBTN.GetComponent<Image>().color = cost <= CurrencyHandler.Instance.CurrentAmount ? canBuy : cantBuy;
    }

    private void BuyItem()
    {
        if (!TutorialHub.Instance.TutorialCompleted) TutorialHub.Instance.countItemBuy++;
        CurrencyHandler.Instance.SubtractCurrency(cost);
        SortInventoryBuyHandler.Instance.AddItemToHandler(_item);
        PCHandler.Instance.ClickSound();
    }

    private void LockSettings()
    {
        if(unlocked) return;

        purchaseBTN.gameObject.SetActive(false);
        unlockBTN.gameObject.SetActive(true);
        unlockBTN.GetComponentInChildren<TextMeshProUGUI>().text = costToUnlock.ToString();
        unlockBTN.onClick.AddListener(UnlockItem);
        iconCurrency.sprite = badItem ? badCurrency : goodCurrency;
    }
    
    private void UnlockItem()
    {
        if (!badItem)
        {
            if(costToUnlock > GoodEssencesHandler.Instance.CurrentAmount) return;
            GoodEssencesHandler.Instance.SubtractCurrency(costToUnlock);
        }
        else
        {
            if (costToUnlock > BadEssencesHandler.Instance.CurrentAmount) return;
            BadEssencesHandler.Instance.SubtractCurrency(costToUnlock);
        }
        PlayerPrefs.SetInt(item + "Unlocked", 1);
        PlayerPrefs.Save();
        unlocked = true;
        purchaseBTN.gameObject.SetActive(true);
        unlockBTN.gameObject.SetActive(false);
        PCHandler.Instance.ClickSound();
        ShowText();
        CheckColor();
        CheckInteractable();
    }
    
    private void CheckPrefs()
    {
        if(unlocked) return;
        var pref = item + "Unlocked";
        var result = PlayerPrefs.GetInt(pref);
        unlocked = result == 1;
    }
    
}
