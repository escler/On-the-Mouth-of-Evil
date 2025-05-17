using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCHandler : MonoBehaviour
{
    public Button goodShopApp, badShopApp, closeGoodShop, closeBadShop, testBTN, testBTN2, testBTN3;
    public GameObject goodShopGO, badShopGO;
    public AudioSource clickSound;
    public static PCHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        goodShopApp.onClick.AddListener(OpenGoodShop);
        badShopApp.onClick.AddListener(OpenBadShop);
        closeGoodShop.onClick.AddListener(CloseGoodShop);
        closeBadShop.onClick.AddListener(CloseBadShop);
        testBTN.onClick.AddListener(AddCurrency);
        testBTN2.onClick.AddListener(AddGoodCurrency);
        testBTN3.onClick.AddListener(AddBadCurrency);
        CheckBadShopEnable();
    }

    public void ClickSound()
    {
        clickSound.Play();
    }

    private void CheckBadShopEnable()
    {
        var badCount = PlayerPrefs.HasKey("BadPath") ? PlayerPrefs.GetInt("BadPath") : 0;
        print(badCount + "Bad Count");
        badShopApp.interactable = badCount > 0;
        var color = badShopApp.GetComponent<Image>().color;
        color.a = badCount > 0 ? 1f : .7f;
        badShopApp.GetComponent<Image>().color = color;
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
        ClickSound();
        goodShopGO.SetActive(!goodShopGO.activeInHierarchy);
    }

    private void OpenBadShop()
    {
        ClickSound();
        badShopGO.SetActive(!badShopGO.activeInHierarchy);
    }

    private void CloseGoodShop()
    {
        ClickSound();
        goodShopGO.SetActive(false);
    }

    private void CloseBadShop()
    {
        ClickSound();
        badShopGO.SetActive(false);
    }

    private void AddCurrency()
    {
        ClickSound();
        CurrencyHandler.Instance.AddCurrency(20);
    }
    
    private void AddGoodCurrency()
    {
        ClickSound();
        GoodEssencesHandler.Instance.AddCurrency(50);
    }
    
    private void AddBadCurrency()
    {
        ClickSound();
        BadEssencesHandler.Instance.AddCurrency(50);
    }
}
