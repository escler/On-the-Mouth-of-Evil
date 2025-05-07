using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadShopHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] items;

    private void OnEnable()
    {
        CheckItemsEnable();
    }

    private void CheckItemsEnable()
    {
        var goodPath = PlayerPrefs.HasKey("BadPath") ? PlayerPrefs.GetInt("BadPath") : 0;
        for (int i = 0; i < items.Length; i++)
        {
            if (i >= goodPath) break;
            items[i].SetActive(true);
        }
    }
}
