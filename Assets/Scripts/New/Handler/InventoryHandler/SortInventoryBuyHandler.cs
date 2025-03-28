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
                LighterHandler.Instance.AddLighter(item.gameObject); 
                break;
            case "Bible":
                BibleHandler.Instance.AddBible(item.gameObject);
                break;
        }
        
    }
}
