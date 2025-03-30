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

    public void GetHandler(Item item)
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
}
