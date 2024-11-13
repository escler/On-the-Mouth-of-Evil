using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    public int capacity;
    public Item selectedItem;
    public Item[][] inventories;
    public int countSelected;
    private KeyCode _key;
    private int _inventorySelect;
    public bool cantSwitch;

    public Item[] inventory, hubInventory, enviromentInventory;
    public Item specialItem;
    private int countHub, countEnviroment;

    public int InventorySelected => _inventorySelect;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        inventories = new Item[2][];
        hubInventory = new Item[capacity + 1];
        enviromentInventory = new Item[capacity];
        inventories[0] = hubInventory;
        inventories[1] = enviromentInventory;
        _inventorySelect = 0;
        inventory = inventories[_inventorySelect];
        countSelected = 0;
        StartCoroutine(DelayFunction());
        SceneManager.sceneLoaded += ClearInventories;
        print(inventory.Length);
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= ClearInventories;
    }

    private void ClearInventories(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (SceneManager.GetActiveScene().name != "Hub") return;
        
        foreach (var inv in inventories)
        {
            for (int i = 0; i < inv.Length; i++)
            {
                if (inv[i] == null) continue;
                InventoryUI.Instance.DeleteUI(i, inv[i].category);
                Destroy(inv[i].gameObject);
                inv[i] = null;
            }
        }

        var slidersUI = InventoryUI.Instance.fillGO.transform;

        for (int i = 0; i < slidersUI.childCount; i++)
        {
            slidersUI.GetChild(i).GetComponent<SliderUI>().ClearSubscripcion();
        }

        countEnviroment = 0;
        countHub = 0;
        cantSwitch = false;

    }


    IEnumerator DelayFunction()
    {
        yield return new WaitForSeconds(.1f);
        InventoryUI.Instance.ChangeInventorySelected(_inventorySelect);
        ChangeSelectedItem(countSelected);
    }

    private void Update()
    {
        if (cantSwitch) return;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) ChangeSelectedItem(countSelected + 1);
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) ChangeSelectedItem(countSelected - 1);

        if (Input.GetButtonDown("Drop")) DropItem(selectedItem, countSelected);

        if (Input.GetButtonDown("1")) ChangeSelectedItem(0);
        if (Input.GetButtonDown("2")) ChangeSelectedItem(1);
        if (Input.GetButtonDown("3")) ChangeSelectedItem(2);
        if (Input.GetButtonDown("4")) ChangeSelectedItem(3);
        if (Input.GetButtonDown("5"))
        {
            if (InventorySelected == 1) return;
            ChangeSelectedItem(4);
        }
        
        if(Input.GetKeyDown(KeyCode.Tab)) ChangeInventoryType();
    }

    public void AddSpecialItem(Item i)
    {
        i.gameObject.SetActive(false);
        if (hubInventory[4] == null)
        {
            hubInventory[4] = i;
            InventoryUI.Instance.ChangeItemUI(i,4,ItemCategory.hubItem);
        }
        else
        {
            var actualItemInSlot = hubInventory[4];
            DropItem(actualItemInSlot, 4);
            hubInventory[4] = i;
            InventoryUI.Instance.ChangeItemUI(i,4,ItemCategory.hubItem);
        }
        
        i.transform.SetParent(PlayerHandler.Instance.handPivot);
        i.transform.localPosition = Vector3.zero;
        i.transform.localEulerAngles = i.angleHand;
        i.GetComponent<BoxCollider>().enabled = false;
        i.GetComponent<Rigidbody>().isKinematic = true;
        ChangeSelectedItem(countSelected);
    }
    
    public void AddItem(Item i, ItemCategory category)
    {
        var inventoryAssigned = category == ItemCategory.hubItem ? inventories[0] : inventories[1];
        var countInventoryAssigned = category == ItemCategory.hubItem ? countHub : countEnviroment;

        i.gameObject.SetActive(false);
        if (countInventoryAssigned < capacity)
        {
            for (int j = 0; j < capacity; j++)
            {
                if (inventoryAssigned[j] != null) continue;
                inventoryAssigned[j] = i;
                countInventoryAssigned++;
                InventoryUI.Instance.ChangeItemUI(i,j,category);
                break;
            }
        }
        else
        {
            var actualItemInSlot = inventoryAssigned[countSelected];
            DropItem(actualItemInSlot, countSelected);
            inventoryAssigned[countSelected] = i;
            ChangeSelectedItem(countSelected);
            InventoryUI.Instance.ChangeItemUI(i,countSelected,category);
        }
        
        i.transform.SetParent(PlayerHandler.Instance.handPivot);
        i.transform.localPosition = Vector3.zero;
        i.transform.localEulerAngles = i.angleHand;
        i.GetComponent<BoxCollider>().enabled = false;
        i.GetComponent<Rigidbody>().isKinematic = true;
        ChangeSelectedItem(countSelected);

        if (category == ItemCategory.hubItem) countHub = countInventoryAssigned;
        else countEnviroment = countInventoryAssigned;
    }


    public void DropItem(Item i, int index)
    {
        if (i == null) return;
        if (index == 4)
        {
            i.transform.parent = null;
            i.GetComponent<BoxCollider>().enabled = true;
            i.GetComponent<Rigidbody>().isKinematic = false;
            i.transform.localScale = Vector3.one;
            i.OnDropItem();
            hubInventory[index] = null;
            if(countSelected == 4) selectedItem = null;
            InventoryUI.Instance.DeleteUI(index,i.category);
            return;
        }
        var count = i.category == ItemCategory.hubItem ? countHub : countEnviroment;
        var categoryInventory = i.category == ItemCategory.hubItem ? inventories[0] : inventories[1];
        if(index != 4) count--;
        if (i.category == ItemCategory.hubItem) countHub = count;
        else countEnviroment = count;
        i.transform.parent = null;
        i.GetComponent<BoxCollider>().enabled = true;
        i.GetComponent<Rigidbody>().isKinematic = false;
        i.transform.localScale = Vector3.one;
        i.OnDropItem();
        categoryInventory[countSelected] = null;
        selectedItem = null;
        InventoryUI.Instance.DeleteUI(index,i.category);
    }

    private void ChangeSelectedItem(int index)
    {
        if (index >= inventory.Length) index = 0;
        else if (index < 0) index = inventory.Length - 1;

        countSelected = index;
        InventorySelectorUI.Instance.OnChangeSelection();
        if (selectedItem == inventory[index]) return;
        if(selectedItem != null) selectedItem.OnDeselectItem();
        ChangeItemState(selectedItem, false);
        selectedItem = inventory[index];
        ChangeItemState(selectedItem, true);
        if(selectedItem != null) selectedItem.OnSelectItem();
    }

    private void ChangeItemState(Item actualItem, bool showState)
    {
        if (actualItem == null) return;
        actualItem.gameObject.SetActive(showState);
    }

    public void ChangeInventoryType()
    {
        if (_inventorySelect == 0)
        {
            _inventorySelect = 1;
            inventory = inventories[_inventorySelect];
            InventoryUI.Instance.ChangeInventorySelected(_inventorySelect);
            if (countSelected == 5) countSelected = 3;
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
