using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salt : Item
{
    public override void OnInteract(bool hit, RaycastHit i)
    {
        base.OnInteract(hit, i);
        if (!hit) return;

        if (i.transform.TryGetComponent(out Door door))
        {
            door.BlockDoor();
            Inventory.Instance.DropItem();
            Destroy(gameObject);
        }
    }
}
