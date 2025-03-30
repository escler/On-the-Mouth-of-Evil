using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemHandler : MonoBehaviour
{
    protected int count;
    public int Count => count;

    public int countMax;

    public Item handlerItem;
    public virtual void AddItem(GameObject item){}
    
    public virtual void RemoveItem(GameObject item){}

}
