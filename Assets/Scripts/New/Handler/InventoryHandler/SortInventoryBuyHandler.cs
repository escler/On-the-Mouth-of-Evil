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
        
        DontDestroyOnLoad(this);
        Instance = this;
    }

    public void AddItemToHandler(Item item)
    {
        switch (item.itemName)
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

    public InventoryItemHandler GetHandler(Item item)
    {
        return item.itemName switch
        {
            "Lighter" => LighterHandler.Instance,
            "Bible" => BibleHandler.Instance,
            "Cross" => CrossHandler.Instance,
            "Salt" => SaltHandler.Instance,
            "Voodoo Doll" => VoodooHandler.Instance,
            "Rosary" => RosaryHandler.Instance,
            _ => null
        };
    }

    public void SaveCount(string itemName, bool sum)
    {
        var name = itemName switch
        {
            "Lighter" => "Lighter",
            "Bible" => "Bible",
            "Cross" => "Cross",
            "Salt" => "Salt",
            "Voodoo Doll" => "Voodoo",
            "Rosary" => "Rosary",
            _ => ""
        };

        name += "Count";
        
        var count = PlayerPrefs.GetInt(name);
        
        count = sum ? count + 1 : count - 1;
        
        PlayerPrefs.SetInt(name, count);
        PlayerPrefs.Save();
    }
}
