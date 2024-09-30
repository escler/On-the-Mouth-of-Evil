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
        inventory = new Item[capacity];
        hubInventory = new Item[capacity];
        enviromentInventory = new Item[capacity];
        inventories[0] = hubInventory;
        inventories[1] = enviromentInventory;
        _inventorySelect = 0;
        inventory = inventories[_inventorySelect];
        countSelected = 0;
        StartCoroutine(DelayFunction());
        SceneManager.sceneLoaded += ClearInventories;
    }

    private void ClearInventories(Scene scene, LoadSceneMode loadSceneMode)
    {
        print("ALo");
        if (SceneManager.GetActiveScene().name != "Hub") return;
        
        foreach (var inv in inventories)
        {
            for (int i = 0; i < inv.Length; i++)
            {
                if (inv[i] != null)
                {
                    Destroy(inv[i].gameObject);
                    inv[i] = null;
                    ChangeUI(i, inv[i].category);
                }
            }
        }

        countEnviroment = 0;
        countHub = 0;

    }


    IEnumerator DelayFunction()
    {
        yield return new WaitForSeconds(.1f);
        InventoryUI.Instance.ChangeInventorySelected(_inventorySelect);
        ChangeSelectedItem(countSelected);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) SceneManager.LoadScene("Hub");
        
        if (cantSwitch) return;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) ChangeSelectedItem(countSelected + 1);
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) ChangeSelectedItem(countSelected - 1);

        if (Input.GetButtonDown("Drop")) DropItem(selectedItem, countSelected);

        if (Input.GetButtonDown("1")) ChangeSelectedItem(0);
        if (Input.GetButtonDown("2")) ChangeSelectedItem(1);
        if (Input.GetButtonDown("3")) ChangeSelectedItem(2);
        if (Input.GetButtonDown("4")) ChangeSelectedItem(3);
        
        if(Input.GetKeyDown(KeyCode.Tab)) ChangeInventoryType();
    }

    /*public void AddItem(Item i, ItemCategory category)
    {
        var categoryInventory = category == ItemCategory.hubItem ? inventories[0] : inventories[1];
        var count = category == ItemCategory.hubItem ? countHub : countEnviroment;
        if (count < capacity)
        {
            i.transform.SetParent(PlayerHandler.Instance.handPivot);
            //i.transform.localScale = Vector3.one;
            i.transform.localPosition = Vector3.zero;
            for (int j = 0; j < categoryInventory.Length; j++)
            {
                if (categoryInventory[j] != null) continue;

                categoryInventory[j] = i.GetComponent<Item>();
                categoryInventory[j].GetComponent<BoxCollider>().enabled = false;
                categoryInventory[j].GetComponent<Rigidbody>().isKinematic = true;

                ChangeUI(j, category);
                break;
            }

            i.gameObject.SetActive(false);
            count++;
            if (category == ItemCategory.hubItem) countHub = count;
            else countEnviroment = count;
            ChangeSelectedItem(countSelected);
            return;
        }

        if(inventories[_inventorySelect] == categoryInventory)DropItem(selectedItem);
        else
        {
            var item = categoryInventory[countSelected];
            item.gameObject.SetActive(true);
            DropItem(item);
        }
        i.transform.SetParent(PlayerHandler.Instance.handPivot);
        i.transform.localScale = Vector3.one;
        i.transform.localPosition = Vector3.zero;

        categoryInventory[countSelected] = i.GetComponent<Item>();
        categoryInventory[countSelected].GetComponent<BoxCollider>().enabled = false;
        categoryInventory[countSelected].GetComponent<Rigidbody>().isKinematic = true;

        ChangeUI(countSelected, category);

        i.gameObject.SetActive(false);
        count++;
        if (category == ItemCategory.hubItem) countHub = count;
        else countEnviroment = count;
        ChangeSelectedItem(countSelected);
    }*/

    public void AddItem(Item i, ItemCategory category)
    {
        var inventoryAssigned = category == ItemCategory.hubItem ? inventories[0] : inventories[1];
        var countInventoryAssigned = category == ItemCategory.hubItem ? countHub : countEnviroment;

        i.gameObject.SetActive(false);
        if (countInventoryAssigned < capacity)
        {
            for (int j = 0; j < inventoryAssigned.Length; j++)
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
        i.GetComponent<BoxCollider>().enabled = false;
        i.GetComponent<Rigidbody>().isKinematic = true;
        ChangeSelectedItem(countSelected);

        print(countInventoryAssigned);
        if (category == ItemCategory.hubItem) countHub = countInventoryAssigned;
        else countEnviroment = countInventoryAssigned;
    }


    public void DropItem(Item i, int index)
    {
        if (i == null) return;
        var count = i.category == ItemCategory.hubItem ? countHub : countEnviroment;
        var categoryInventory = i.category == ItemCategory.hubItem ? inventories[0] : inventories[1];
        count--;
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

    public void ChangeUI(int index, ItemCategory category)
    {
        InventoryUI.Instance.ChangeItemUI(inventory[index], index, category);
        //InventoryUI.Instance.ChangeSelectedItem(countSelected);
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
