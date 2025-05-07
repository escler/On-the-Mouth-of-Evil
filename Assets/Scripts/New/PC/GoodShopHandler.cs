using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodShopHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] items;

    private void OnEnable()
    {
        CheckItemsEnable();
    }

    private void CheckItemsEnable()
    {
        var goodPath = PlayerPrefs.HasKey("GoodPath") ? PlayerPrefs.GetInt("GoodPath") : 0;
        for (int i = 0; i < items.Length; i++)
        {
            if (i >= goodPath) break;
            items[i].SetActive(true);
        }
    }
}
