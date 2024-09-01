using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }
    public GameObject emptyUI;
    private int _indexSelectedItem;
    [SerializeField] private TextMeshProUGUI nameItemSelected;
    
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
            var uiElement = Instantiate(inventory[i] != null ? inventory[i].uiElement : emptyUI);
            uiElement.transform.SetParent(transform);
            uiElement.transform.localScale = Vector3.one;
        }

        _indexSelectedItem = Inventory.Instance.countSelected;

    }

    public void ChangeItemUI(Item i, int index)
    {
        var actualElement = transform.GetChild(index);
        actualElement.SetParent(null);
        GameObject uiElement = Instantiate(i != null ? i.uiElement : emptyUI);
        uiElement.transform.localScale = Vector3.one;
        uiElement.transform.SetParent(transform);
        uiElement.transform.SetSiblingIndex(index);
        Destroy(actualElement.gameObject);
    }

    public void ChangeSelectedItem(int index)
    {
        if (transform.childCount <= _indexSelectedItem) return;
        transform.GetChild(_indexSelectedItem).GetComponent<Image>().color = Color.white;
        _indexSelectedItem = index;
        transform.GetChild(_indexSelectedItem).GetComponent<Image>().color = Color.yellow;
        nameItemSelected.text = Inventory.Instance.selectedItem == null ? "" : Inventory.Instance.selectedItem.itemName;
    }
}
