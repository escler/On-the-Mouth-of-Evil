using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scalpel : Item
{
    [SerializeField] private GameObject aura;

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
        aura.SetActive(false);
    }

    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (!hit) return;

        if (i.transform.TryGetComponent(out BodyPuzzle body))
        {
            if (!body.bloodDrained)
            {
                DialogHandler.Instance.ChangeText("Too much blood. I need to drain it first.");
                return;
            }
            body.OpenBody();
            Inventory.Instance.DropItem(this, Inventory.Instance.countSelected);
            Destroy(gameObject);
            
        }
    }
    
    public override bool CanInteractWithItem()
    {
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();

        if (!rayConnected) return false;
        
        if (ObjectDetector.Instance.InteractText()) return true;
        if (ray.transform.TryGetComponent(out BodyPuzzle body))
        {
            return true;
        } 
        return false;
    }
}
