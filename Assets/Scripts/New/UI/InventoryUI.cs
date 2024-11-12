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
    public GameObject hubInventoryUI, enviromentInventoryUI, actualInventoryUI, specialInventoryUI, blurHub, blurEnviroment, blurSpecial;
    private int _indexSelectedItem;
    [SerializeField] private TextMeshProUGUI nameItemSelected;
    public Transform activePos, deactivePos;
    public GameObject fillGO;
    
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
        InitializeUI();
    }

    public void InitializeUI()
    {
        _indexSelectedItem = Inventory.Instance.countSelected;
        ChangeSelectedItem(_indexSelectedItem);
    }

    public void ChangeItemUI(Item i, int index, ItemCategory category)
    {
        if (index == 4)
        {
            var uiSpecial = specialInventoryUI.transform.GetChild(0);
            GameObject uiSpecialElement = Instantiate(i.uiElement);
            uiSpecialElement.transform.SetParent(uiSpecial);
            uiSpecialElement.transform.localPosition = Vector3.zero;
            uiSpecialElement.transform.localScale = Vector3.one;
            return;
        }
        var actualElement = category == ItemCategory.hubItem ? 
            hubInventoryUI.transform.GetChild(index + 2) : enviromentInventoryUI.transform.GetChild(index);
        
        GameObject uiElement = Instantiate(i.uiElement);
        uiElement.transform.SetParent(actualElement);
        uiElement.transform.localPosition = Vector3.zero;
        uiElement.transform.localScale = Vector3.one;
    }

    public void DeleteUI(int index, ItemCategory category)
    {
        if (index == 4)
        {
            
            var uiSpecial = specialInventoryUI.transform.GetChild(0).GetChild(0);
            uiSpecial.SetParent(null);
            Destroy(uiSpecial.gameObject);
            return;
        }
        var inventory = category == ItemCategory.hubItem ? hubInventoryUI : enviromentInventoryUI;

        index = category == ItemCategory.hubItem ? index + 2 : index;
        var ui = inventory.transform.GetChild(index).GetChild(0);
        ui.SetParent(null);
        Destroy(ui.gameObject);
    }

    public void ChangeSelectedItem(int index)
    {
        if (actualInventoryUI.transform.childCount <= _indexSelectedItem) return;
     //       actualInventoryUI.transform.GetChild(_indexSelectedItem).GetComponent<Image>().color = Color.white;
        _indexSelectedItem = index;
       //    actualInventoryUI.transform.GetChild(_indexSelectedItem).GetComponent<Image>().color = Color.yellow;
        nameItemSelected.text = Inventory.Instance.selectedItem == null ? "" : Inventory.Instance.selectedItem.itemName;
    }

    public void ChangeInventorySelected(int category)
    {
        switch (category)
        {
            case 0:
                hubInventoryUI.transform.localPosition = activePos.localPosition;
                hubInventoryUI.transform.localScale = activePos.localScale;
                specialInventoryUI.transform.localPosition = activePos.localPosition;
                specialInventoryUI.transform.localScale = new Vector3(2.3f, 2.3f, 2.3f);
                enviromentInventoryUI.transform.localPosition = deactivePos.localPosition;
                enviromentInventoryUI.transform.localScale = deactivePos.localScale;
                actualInventoryUI = hubInventoryUI;
                blurHub.SetActive(false);
                blurSpecial.SetActive(false);
                blurEnviroment.SetActive(true);
                break;
            case 1:
                hubInventoryUI.transform.localPosition = deactivePos.localPosition;
                hubInventoryUI.transform.localScale = deactivePos.localScale;
                specialInventoryUI.transform.localPosition = deactivePos.localPosition;
                specialInventoryUI.transform.localScale = deactivePos.localScale;
                enviromentInventoryUI.transform.localPosition = activePos.localPosition;
                enviromentInventoryUI.transform.localScale = activePos.localScale;
                actualInventoryUI = enviromentInventoryUI;
                blurHub.SetActive(true);
                blurSpecial.SetActive(true);
                blurEnviroment.SetActive(false);
                break;
        }
    }
}
