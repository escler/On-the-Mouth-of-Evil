using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public string objectName;
    public GameObject objectPrefab, uiElement;
    
    public virtual void OnGrabItem()
    {
        Inventory.Instance.AddItem(this);
        Destroy(gameObject);
    }

    public virtual void OnInteract()
    {
        
    }
}
