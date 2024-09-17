using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }
    public GameObject emptyUI, hubInventoryUI, enviromentInventoryUI, actualInventoryUI;
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
        actualInventoryUI = hubInventoryUI;
    }

    private void Start()
    {
        var inventory = Inventory.Instance.hubInventory;
        
        for (int i = 0; i < inventory.Length; i++)
        {
            var uiElement = Instantiate(inventory[i] != null ? inventory[i].uiElement : emptyUI);
            uiElement.transform.SetParent(hubInventoryUI.transform);
            uiElement.transform.localScale = Vector3.one;
        }

        inventory = Inventory.Instance.enviromentInventory;

        for (int i = 0; i < inventory.Length; i++)
        {
            var uiElement = Instantiate(inventory[i] != null ? inventory[i].uiElement : emptyUI);
            uiElement.transform.SetParent(enviromentInventoryUI.transform);
            uiElement.transform.localScale = Vector3.one;
        }

        _indexSelectedItem = Inventory.Instance.countSelected;
        ChangeSelectedItem(_indexSelectedItem);

    }

    public void ChangeItemUI(Item i, int index)
    {
        var actualElement = actualInventoryUI.transform.GetChild(index);
        actualElement.SetParent(null);
        GameObject uiElement = Instantiate(i != null ? i.uiElement : emptyUI);
        uiElement.transform.localScale = Vector3.one;
        uiElement.transform.SetParent(actualInventoryUI.transform);
        uiElement.transform.SetSiblingIndex(index);
        Destroy(actualElement.gameObject);
    }

    public void ChangeSelectedItem(int index)
    {
        if (actualInventoryUI.transform.childCount <= _indexSelectedItem) return;
        actualInventoryUI.transform.GetChild(_indexSelectedItem).GetComponent<Image>().color = Color.white;
        _indexSelectedItem = index;
        actualInventoryUI.transform.GetChild(_indexSelectedItem).GetComponent<Image>().color = Color.yellow;
        nameItemSelected.text = Inventory.Instance.selectedItem == null ? "" : Inventory.Instance.selectedItem.itemName;
    }

    public void ChangeInventorySelected(int category)
    {
        switch (category)
        {
            case 0:
                hubInventoryUI.SetActive(true);
                enviromentInventoryUI.SetActive(false);
                actualInventoryUI = hubInventoryUI;
                var inventoryHub = Inventory.Instance.hubInventory;

                for (int i = 0; i < inventoryHub.Length; i++)
                {
                    ChangeItemUI(inventoryHub[i], i);
                }
                break;
            case 1:
                hubInventoryUI.SetActive(false);
                enviromentInventoryUI.SetActive(true);
                actualInventoryUI = enviromentInventoryUI;
                var inventoryEnviroment = Inventory.Instance.enviromentInventory;
                for (int i = 0; i < inventoryEnviroment.Length; i++)
                {
                    ChangeItemUI(inventoryEnviroment[i], i);
                }
                break;
        }
    }
}
