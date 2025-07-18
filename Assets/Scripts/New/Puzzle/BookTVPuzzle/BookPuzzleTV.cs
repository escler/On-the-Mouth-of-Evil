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
        PuzzleBookTV.Instance.HideGlows();
    }
    
    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (!hit) return;
        
        if (i.transform.TryGetComponent(out BookSpot bookSpot))
        {
            Inventory.Instance.DropItem(this, Inventory.Instance.countSelected);
            bookSpot.PlaceBook(this);
        }
    }

    public override void OnSelectItem()
    {
        base.OnSelectItem();
        PuzzleBookTV.Instance.ShowGlows();
    }

    public override void OnDeselectItem()
    {
        base.OnDeselectItem();
        PuzzleBookTV.Instance.HideGlows();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();
        ChangeCrossHair();
        ObjectDetector.Instance.uiInteractionText.SetActive(CanInteractWithItem());
        if (Input.GetButtonDown("Interact"))
        {
            OnInteract(rayConnected, ray);
        }
    }

}
