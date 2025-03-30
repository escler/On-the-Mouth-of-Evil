using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class SortInventoryBuyHandler : MonoBehaviour
{
    public static SortInventoryBuyHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);
            return;
        }

        Instance = this;
    }

    public void AddItemToHandler(Item item)
    {
        switch (item.name)
        {
            case "Lighter":
                LighterHandler.Instance.AddItem(item.gameObject); 
                break;
            case "Bible":
                BibleHandler.Instance.AddItem(item.gameObject);
                break;
            case "Cross":
                CrossHandler.Instance.AddItem(item.gameObject);
                break;
            case "Salt":
                SaltHandler.Instance.AddItem(item.gameObject);
                break;
            case "Voodoo Doll":
                VoodooHandler.Instance.AddItem(item.gameObject);
                break;
            case "Rosary":
                RosaryHandler.Instance.AddItem(item.gameObject);
                break;
        }
    }

    public void RemoveItemFromHandler(Item item)
    {
        switch (item.name)
        {
            case "Lighter":
                LighterHandler.Instance.RemoveItem(item.gameObject);
                break;
            case "Bible":
                BibleHandler.Instance.RemoveItem(item.gameObject);
                break;
            case "Cross":
                CrossHandler.Instance.RemoveItem(item.gameObject);
                break;
            case "Salt":
                SaltHandler.Instance.RemoveItem(item.gameObject);
                break;
            case "Voodoo Doll":
                VoodooHandler.Instance.RemoveItem(item.gameObject);
                break;
            case "Rosary":
                RosaryHandler.Instance.RemoveItem(item.gameObject);
                break;
        }
    }

    public InventoryItemHandler GetHandler(Item item)
    {
        switch (item.name)
        {
            case "Lighter":
                return LighterHandler.Instance;
            case "Bible":
                return BibleHandler.Instance;
            case "Cross":
                return CrossHandler.Instance;
            case "Salt":
                return SaltHandler.Instance;
            case "Voodoo Doll":
                return VoodooHandler.Instance;
            case "Rosary":
                return RosaryHandler.Instance;
        }
        
        return null;
    }
}
