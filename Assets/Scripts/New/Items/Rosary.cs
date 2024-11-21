using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rosary : Item
{
    public float chanceToProtect;
    
    public override void OnGrabItem()
    {
        Inventory.Instance.AddSpecialItem(this);
        if (GetComponentInChildren<SkinnedMeshRenderer>() == null)
        {
            GetComponentInChildren<MeshRenderer>().gameObject.layer = 18;
        }
        else
        {
            GetComponentInChildren<SkinnedMeshRenderer>().gameObject.layer = 18;
        }
    }
    
    public override void OnDropItem()
    {
        gameObject.SetActive(true);
        GetComponentInChildren<MeshRenderer>().gameObject.layer = 1;
    }

    public bool RosaryProtect()
    {
        float random = Random.Range(0, 1);
        bool success = random <= chanceToProtect;
        
        return success;
    }

}
