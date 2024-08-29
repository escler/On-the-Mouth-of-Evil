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
    
    public void OnGrabItem()
    {
        Inventory.Instance.AddItem(this);
    }

    public virtual void OnInteract()
    {
        
    }

    public virtual void OnInteract(bool hit, RaycastHit i)
    {
        print(canUse ? "Usado" : "No Usado");
        canUse = false;
    }
}
