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

    public delegate void UpdateCount();
    
    public event UpdateCount OnUpdateCount;
    protected void SaveCount(bool sum)
    {
        print("Inve " + handlerItem.name);
        SortInventoryBuyHandler.Instance.SaveCount(handlerItem.name, sum);
        OnUpdateCount?.Invoke();
    }
}
