using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bible : Item, IBurneable
{
    private bool _placed;
    public void OnBurn()
    {
        if (!_placed) return;
        print("Me quemo");
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public override void OnInteract(bool hit, RaycastHit i)
    {
        Inventory.Instance.DropItem();
        _placed = true;
        base.OnInteract(hit,i);
    }
}
