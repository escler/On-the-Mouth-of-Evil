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
        items[0].SetActive(PlayerPrefs.GetInt("VoodooAvaible") == 1);
    }
}
