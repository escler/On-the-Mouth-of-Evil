using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    public int capacity;
    public Item selectedItem;
    public int countSelected;
    private KeyCode _key;

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
        countSelected = 0;
        StartCoroutine(DelayFunction());
    }
    

    IEnumerator DelayFunction()
    {
        yield return new WaitForSeconds(.1f);
        ChangeSelectedItem(countSelected);
    }

    private void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0f) ChangeSelectedItem(countSelected + 1);
        else if(Input.GetAxis("Mouse ScrollWheel") < 0f) ChangeSelectedItem(countSelected - 1);
        
        if(Input.GetButtonDown("Drop")) DropItem();
        
        if(Input.GetButtonDown("1")) ChangeSelectedItem(0);
        if(Input.GetButtonDown("2")) ChangeSelectedItem(1);
        if(Input.GetButtonDown("3")) ChangeSelectedItem(2);
        if(Input.GetButtonDown("4")) ChangeSelectedItem(3);
    }

    public bool AddItem(Item i)
    {
        if (count < capacity)
        {
            i.transform.SetParent(PlayerHandler.Instance.handPivot);
            i.transform.localPosition = Vector3.zero;
            for (int j = 0; j < inventory.Length; j++)
            {
                if (inventory[j] != null) continue;

                inventory[j] = i.GetComponent<Item>();
                inventory[j].GetComponent<BoxCollider>().enabled = false;
                inventory[j].GetComponent<Rigidbody>().isKinematic = true;

                ChangeUI(j);
                break;
            }

            i.gameObject.SetActive(false);
            count++;
            ChangeSelectedItem(countSelected);
            return true;
        }
        
            DropItem();
            i.transform.SetParent(PlayerHandler.Instance.handPivot);
            i.transform.localPosition = Vector3.zero;
            
            inventory[countSelected] = i.GetComponent<Item>();
            inventory[countSelected].GetComponent<BoxCollider>().enabled = false;
            inventory[countSelected].GetComponent<Rigidbody>().isKinematic = true;

            ChangeUI(countSelected);
            
            i.gameObject.SetActive(false);
            count++;
            ChangeSelectedItem(countSelected);
            return true;
        
    }

    public void DropItem()
    {
        if (selectedItem == null) return;
        count--;
        selectedItem.transform.parent = null;
        selectedItem.GetComponent<BoxCollider>().enabled = true;
        selectedItem.GetComponent<Rigidbody>().isKinematic = false;
        selectedItem.transform.localScale = Vector3.one / 2;
        inventory[countSelected] = null;
        selectedItem = inventory[countSelected];
        ChangeUI(countSelected);
    }

    private void ChangeSelectedItem(int index)
    {
        if (index >= inventory.Length) index = 0;
        else if (index < 0) index = inventory.Length - 1;

        countSelected = index;
        ChangeItemState(selectedItem,false);
        selectedItem = inventory[index];
        ChangeItemState(selectedItem,true);
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
    
}
