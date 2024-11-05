using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipientItem : Item 
{
    public MeshRenderer tapa, body;

    public override void OnUpdate()
    {
        base.OnUpdate();
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();
        canInteractWithItem = CanInteractWithItem();
        ChangeCrossHair();
        
        if(Input.GetButtonDown("Interact")) OnInteract(rayConnected,ray);
    }

    public override void OnGrabItem()
    {
        Inventory.Instance.AddItem(this, category);
        tapa.gameObject.layer = 18;
        body.gameObject.layer = 18;
    }

    public override void OnDropItem()
    {
        base.OnDropItem();
        tapa.gameObject.layer = 1;
        body.gameObject.layer = 17;
    }

    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (!hit) return;
        if (i.transform.TryGetComponent(out SaltPuzzleTable table))
        {
            table.PlaceRecipient();
            Inventory.Instance.DropItem(this,Inventory.Instance.countSelected);
            Destroy(gameObject);
        }
    }

    public override bool CanInteractWithItem()
    {
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();

        if (!rayConnected) return false;
        return ray.transform.TryGetComponent(out SaltPuzzleTable table);
    }
}
