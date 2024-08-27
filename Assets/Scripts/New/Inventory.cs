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

    private void Start()
    {
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
    }

    public bool AddItem(Item i)
    {
        if (count < capacity)
        {
            var newObj = Instantiate(i, PlayerHandler.Instance.handPivot, true);
            newObj.transform.localPosition = Vector3.zero;
            for (int j = 0; j < inventory.Length; j++)
            {
                if (inventory[j] != null) continue;

                inventory[j] = newObj.GetComponent<Item>();
                inventory[j].GetComponent<BoxCollider>().enabled = false;
                inventory[j].GetComponent<Rigidbody>().isKinematic = true;

                ChangeUI(j);
                break;
            }

            newObj.gameObject.SetActive(false);
            count++;
            ChangeSelectedItem(countSelected);
            return true;
        }
        else
        {
            DropItem();
            var newObj = Instantiate(i, PlayerHandler.Instance.handPivot, true);
            newObj.transform.localPosition = Vector3.zero;
            
            inventory[countSelected] = newObj.GetComponent<Item>();
            inventory[countSelected].GetComponent<BoxCollider>().enabled = false;
            inventory[countSelected].GetComponent<Rigidbody>().isKinematic = true;

            ChangeUI(countSelected);
            
            newObj.gameObject.SetActive(false);
            count++;
            ChangeSelectedItem(countSelected);
            return true;
        }
    }

    private void DropItem()
    {
        if (selectedItem == null) return;
        count--;
        var spawnObj = Instantiate(selectedItem, PlayerHandler.Instance.handPivot.position, PlayerHandler.Instance.handPivot.rotation);
        spawnObj.GetComponent<BoxCollider>().enabled = true;
        spawnObj.GetComponent<Rigidbody>().isKinematic = false;
        spawnObj.transform.localScale = Vector3.one / 2;
        inventory[countSelected] = null;
        Destroy(selectedItem.gameObject);
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
