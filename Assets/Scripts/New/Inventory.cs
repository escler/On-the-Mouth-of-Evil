using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    public int capacity;
    public Item selectedItem;
    public Item[][] inventories;
    public int countSelected;
    private KeyCode _key;
    private int _inventorySelect;

    public Item[] inventory, hubInventory, enviromentInventory;
    private int countHub, countEnviroment;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        inventories = new Item[2][];
        inventory = new Item[capacity];
        hubInventory = new Item[capacity];
        enviromentInventory = new Item[capacity];
        inventories[0] = hubInventory;
        inventories[1] = enviromentInventory;
        _inventorySelect = 0;
        inventory = inventories[_inventorySelect];
        countSelected = 0;
        StartCoroutine(DelayFunction());
    }


    IEnumerator DelayFunction()
    {
        yield return new WaitForSeconds(.1f);
        InventoryUI.Instance.ChangeInventorySelected(_inventorySelect);
        ChangeSelectedItem(countSelected);
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) ChangeSelectedItem(countSelected + 1);
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) ChangeSelectedItem(countSelected - 1);

        if (Input.GetButtonDown("Drop")) DropItem();

        if (Input.GetButtonDown("1")) ChangeSelectedItem(0);
        if (Input.GetButtonDown("2")) ChangeSelectedItem(1);
        if (Input.GetButtonDown("3")) ChangeSelectedItem(2);
        if (Input.GetButtonDown("4")) ChangeSelectedItem(3);
        
        if(Input.GetKeyDown(KeyCode.Tab)) ChangeInventoryType();
    }

    public void AddItem(Item i, ItemCategory category)
    {
        var categoryInventory = category == ItemCategory.hubItem ? inventories[0] : inventories[1];
        var count = category == ItemCategory.hubItem ? countHub : countEnviroment;
        if (count < capacity)
        {
            i.transform.SetParent(PlayerHandler.Instance.handPivot);
            i.transform.localScale = Vector3.one;
            i.transform.localPosition = Vector3.zero;
            for (int j = 0; j < categoryInventory.Length; j++)
            {
                if (categoryInventory[j] != null) continue;

                categoryInventory[j] = i.GetComponent<Item>();
                categoryInventory[j].GetComponent<BoxCollider>().enabled = false;
                categoryInventory[j].GetComponent<Rigidbody>().isKinematic = true;

                ChangeUI(j);
                break;
            }

            i.gameObject.SetActive(false);
            count++;
            if (category == ItemCategory.hubItem) countHub = count;
            else countEnviroment = count;
            ChangeSelectedItem(countSelected);
            return;
        }

        DropItem();
        i.transform.SetParent(PlayerHandler.Instance.handPivot);
        i.transform.localScale = Vector3.one;
        i.transform.localPosition = Vector3.zero;

        categoryInventory[countSelected] = i.GetComponent<Item>();
        categoryInventory[countSelected].GetComponent<BoxCollider>().enabled = false;
        categoryInventory[countSelected].GetComponent<Rigidbody>().isKinematic = true;

        ChangeUI(countSelected);

        i.gameObject.SetActive(false);
        count++;
        if (category == ItemCategory.hubItem) countHub = count;
        else countEnviroment = count;
        ChangeSelectedItem(countSelected);

    }

    public void DropItem()
    {
        if (selectedItem == null) return;
        var count = selectedItem.category == ItemCategory.hubItem ? countHub : countEnviroment;
        count--;
        if (selectedItem.category == ItemCategory.hubItem) countHub = count;
        else countEnviroment = count;
        selectedItem.transform.parent = null;
        selectedItem.OnDropItem();
        selectedItem.GetComponent<BoxCollider>().enabled = true;
        selectedItem.GetComponent<Rigidbody>().isKinematic = false;
        selectedItem.transform.localScale = Vector3.one;
        inventory[countSelected] = null;
        selectedItem = inventory[countSelected];
        ChangeUI(countSelected);
    }

    private void ChangeSelectedItem(int index)
    {
        if (index >= inventory.Length) index = 0;
        else if (index < 0) index = inventory.Length - 1;

        countSelected = index;
        ChangeItemState(selectedItem, false);
        selectedItem = inventory[index];
        ChangeItemState(selectedItem, true);
        if(selectedItem != null) selectedItem.OnSelectItem();
        ChangeUI(countSelected);
    }

    private void ChangeItemState(Item actualItem, bool showState)
    {
        if (actualItem == null) return;
        actualItem.gameObject.SetActive(showState);
    }

    public void ChangeUI(int index)
    {
        InventoryUI.Instance.ChangeItemUI(inventory[index], index);
        InventoryUI.Instance.ChangeSelectedItem(countSelected);
    }

    public void ChangeInventoryType()
    {
        if (_inventorySelect == 0)
        {
            _inventorySelect = 1;
            inventory = inventories[_inventorySelect];
            InventoryUI.Instance.ChangeInventorySelected(_inventorySelect);
        }
        else
        {
            _inventorySelect = 0;
            inventory = inventories[_inventorySelect];
            InventoryUI.Instance.ChangeInventorySelected(_inventorySelect);
        }
        
        ChangeSelectedItem(countSelected);
    }

}
