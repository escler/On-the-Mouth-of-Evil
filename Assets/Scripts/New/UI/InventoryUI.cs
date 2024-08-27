using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }
    public GameObject emptyUI;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        var inventory = Inventory.Instance.inventory;

        for (int i = 0; i < inventory.Length; i++)
        {
            var uiElement = Instantiate(emptyUI);
            uiElement.transform.SetParent(transform);
            uiElement.transform.localScale = Vector3.one;
        }
    }

    public void ChangeItemUI(Item i, int index)
    {
        var actualElement = transform.GetChild(index);
        actualElement.SetParent(null);
        var uiElement = Instantiate(i.uiElement);
        uiElement.transform.localScale = Vector3.one;
        uiElement.transform.SetParent(transform);
        uiElement.transform.SetSiblingIndex(index);
        Destroy(actualElement.gameObject);
    }
}
