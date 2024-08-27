using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    public int capacity;

    public Item[] inventory;
    private int count;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        inventory = new Item[capacity];
    }

    public void AddItem(Item i)
    {
        var newObj = Instantiate(i);
        inventory[count] = newObj.GetComponent<Item>();
        InventoryUI.Instance.AddItem(newObj);
        newObj.gameObject.SetActive(false);
    }
    
}
