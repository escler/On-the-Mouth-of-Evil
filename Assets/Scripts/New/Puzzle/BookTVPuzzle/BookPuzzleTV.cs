using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookPuzzleTV : Item
{
    public int number;
    
    public override void OnGrabItem()
    {
        base.OnGrabItem();
        GetComponentInChildren<AuraItem>().onHand = true;
    }

    public override void OnDropItem()
    {
        base.OnDropItem();
        GetComponentInChildren<AuraItem>().onHand = false;
    }
    
    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (!hit) return;
        if (!canInteractWithItem) return;
        
        if (i.transform.TryGetComponent(out BookSpot bookSpot))
        {
            Inventory.Instance.DropItem(this, Inventory.Instance.countSelected);
            bookSpot.PlaceBook(this);
        }
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();
        canInteractWithItem = CanInteractWithItem();
        ObjectDetector.Instance.uiInteractionText.SetActive(CanInteractWithItem());
        if (Input.GetButtonDown("Interact"))
        {
            OnInteract(rayConnected, ray);
        }
    }
    
    public override bool CanInteractWithItem()
    {
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();

        if (!rayConnected) return false;
        if (ObjectDetector.Instance.InteractText()) return true;
        if (ray.transform.TryGetComponent(out BookSpot item))
        {
            if (item.BookPuzzleTV == null) return true;
            return false;
        }
        return false;
    }
}
