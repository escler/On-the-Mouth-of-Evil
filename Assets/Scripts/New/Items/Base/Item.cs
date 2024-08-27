using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public string objectName;
    public GameObject objectPrefab, uiElement;
    
    public virtual void OnGrabItem()
    {
        var itemGrabbed = Inventory.Instance.AddItem(this);
        if (itemGrabbed) Destroy(gameObject);
    }

    public void OnDropItem()
    {
        
    }

    public virtual void OnInteract()
    {
        
    }
}
