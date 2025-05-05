using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mission : Item
{
    public string misionName;

    public Item[] itemsNeededs;

    public override void OnDropItem()
    {
        base.OnDropItem();
        PlayerHandler.Instance.actualMission = null;
    }

    public override void OnGrabItem()
    {
        var inventory = Inventory.Instance.enviromentInventory;
        var missions = inventory.Select(x => x != null && x.itemName == "Mission Level");
        print(missions.Count());
        if (missions.Any())
        {
            var first = missions.First();
            for (int i = 0; i < inventory.Length; i++)
            {
                if(inventory[i] != first) continue;
                Inventory.Instance.DropItem(inventory[i], i);
                Inventory.Instance.AddItem(this, category);
                break;
            }
        }
        else
        {
            Inventory.Instance.AddItem(this, category);
        }
        
        
        if (GetComponentInChildren<SkinnedMeshRenderer>() == null)
        {
            GetComponentInChildren<MeshRenderer>().gameObject.layer = 18;
        }
        else
        {
            GetComponentInChildren<SkinnedMeshRenderer>().gameObject.layer = 18;
        }
    }

    public virtual void OnGrabMission()
    {
    }

    public override void OnInteractItem()
    {
        base.OnInteractItem();
        transform.localScale = Vector3.one;
        OnGrabMission();
    }
}
