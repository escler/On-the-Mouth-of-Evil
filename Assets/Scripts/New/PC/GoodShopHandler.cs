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
        items[0].SetActive(PlayerPrefs.GetInt("RosaryAvaible") == 1);
        items[1].SetActive(PlayerPrefs.GetInt("IncenseAvaible") == 1);

    }
}
