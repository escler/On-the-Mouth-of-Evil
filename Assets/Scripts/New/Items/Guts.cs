using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guts : Item
{
    [SerializeField] private MeshRenderer[] meshes;

    public override void OnGrabItem()
    {
        Inventory.Instance.AddItem(this, category);
        transform.localEulerAngles = angleHand;
        foreach (var mesh in meshes)
        {
            mesh.gameObject.layer = 18;
        }
    }

    public override void OnDropItem()
    {
        gameObject.SetActive(true);
        foreach (var mesh in meshes)
        {
            mesh.gameObject.layer = 1;
        }
    }
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


    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (!hit) return;

        if (i.transform.TryGetComponent(out BadRitual ritual))
        {
            if (!ritual.baitBoxPlaced) return;
            ritual.StartRitual();
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
        if (ray.transform.TryGetComponent(out BadRitual ritual))
        {
            if (ritual.baitBoxPlaced) return true;
        } 
        return false;
    }
}
