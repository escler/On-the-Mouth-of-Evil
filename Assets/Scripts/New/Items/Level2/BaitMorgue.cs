using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitMorgue : Item
{
    private bool _dialogAppear;
    
    public override void OnUpdate()
    {
        base.OnUpdate();

        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();
        canInteractWithItem = CanInteractWithItem();
        ChangeCrossHair();

        if (Input.GetMouseButtonDown(0))
        {
            OnInteract(rayConnected, ray);
        }
    }

    public override void OnGrabItem()
    {
        base.OnGrabItem();
        if (!_dialogAppear)
        {
            DialogHandler.Instance.ChangeText("With this, I could contain the demon… but I’ll need bait strong enough to tempt it.");
            _dialogAppear = true;
        }
        SkullPuzzle.Instance.freezerCollider.enabled = true;
        foreach (var f in SkullPuzzle.Instance.freezerColliders)
        {
            f.enabled = false;
        }
    }


    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (!hit) return;

        if (i.transform.TryGetComponent(out BadRitual ritual))
        {
            Inventory.Instance.DropItem(this, Inventory.Instance.countSelected);
            ritual.baitBoxPlaced = true;
            transform.position = ritual.transform.position + Vector3.up * 0.15f;
            transform.rotation = ritual.transform.rotation;
            transform.parent = ritual.transform;
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    
    public override bool CanInteractWithItem()
    {
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();

        if (!rayConnected) return false;
        
        if (ObjectDetector.Instance.InteractText()) return true;
        if (ray.transform.TryGetComponent(out BadRitual ritual))
        {
            return true;
        } 
        return false;
    }
}
