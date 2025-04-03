using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCHandler : MonoBehaviour
{
    public Button goodShopApp, badShopApp, closeGoodShop, closeBadShop, testBTN, testBTN2, testBTN3;
    public GameObject goodShopGO, badShopGO; 

    private void Awake()
    {
        goodShopApp.onClick.AddListener(OpenGoodShop);
        badShopApp.onClick.AddListener(OpenBadShop);
        closeGoodShop.onClick.AddListener(CloseGoodShop);
        closeBadShop.onClick.AddListener(CloseBadShop);
        testBTN.onClick.AddListener(AddCurrency);
        testBTN2.onClick.AddListener(AddGoodCurrency);
        testBTN3.onClick.AddListener(AddBadCurrency);
    }

    private void OnDestroy()
    {
        goodShopApp.onClick.RemoveAllListeners();
        badShopApp.onClick.RemoveAllListeners();
        closeGoodShop.onClick.RemoveAllListeners();
        closeBadShop.onClick.RemoveAllListeners();
        testBTN.onClick.RemoveAllListeners();
        testBTN2.onClick.RemoveAllListeners();
        testBTN3.onClick.RemoveAllListeners();
    }

    private void OpenGoodShop()
    {
        goodShopGO.SetActive(!goodShopGO.activeInHierarchy);
    }

    private void OpenBadShop()
    {
        badShopGO.SetActive(!badShopGO.activeInHierarchy);
    }

    private void CloseGoodShop()
    {
        goodShopGO.SetActive(false);
    }

    private void CloseBadShop()
    {
        badShopGO.SetActive(false);
    }

    private void AddCurrency()
    {
        CurrencyHandler.Instance.AddCurrency(20);
    }
    
    private void AddGoodCurrency()
    {
        GoodEssencesHandler.Instance.AddCurrency(50);
    }
    
    private void AddBadCurrency()
    {
        BadEssencesHandler.Instance.AddCurrency(50);
    }
}
