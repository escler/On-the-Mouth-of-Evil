using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighter : Item
{
    [SerializeField] private List<Item> _itemsInteractables;
    public GameObject PSIdle;
    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (!hit) return;
        if (i.transform.TryGetComponent(out IBurneable item))
        {
            item.OnBurn();
            canUse = true;
        }
        else
        {
            canUse = false;
        }
        base.OnInteract(hit,i);
    }

    public override void OnSelectItem()
    {
        base.OnSelectItem();
        PSIdle.SetActive(true);
    }

    public override void OnDropItem()
    {
        gameObject.SetActive(true);
        PSIdle.SetActive(false);
    }
}
