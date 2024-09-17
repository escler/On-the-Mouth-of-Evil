using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public string itemName;
    public GameObject objectPrefab, uiElement;
    private bool _canInteract;
    protected bool canUse;
    public string uiText;
    public ItemCategory category;
    
    public void OnGrabItem()
    {
        Inventory.Instance.AddItem(this, category);
    }

    public virtual void OnInteractItem()
    {
        OnGrabItem();
    }

    public virtual void OnSelectItem()
    {
        
    }

    public virtual void OnDropItem()
    {
        
    }

    public virtual void OnInteract(bool hit, RaycastHit i)
    {
        canUse = false;
    }

    public string ShowText()
    {
        return uiText;
    }
}
