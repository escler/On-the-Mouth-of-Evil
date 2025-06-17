using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inciense : Item
{


    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (canInteractWithItem) return;
        if (Enemy.Instance == null) return;
        
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
        }
    }

    public override bool CanInteractWithItem()
    {
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();
        
        if (rayConnected && ObjectDetector.Instance.InteractText()) return true;

        return false;
    }

    public override void OnGrabItem()
    {
        if (GetComponentInChildren<SkinnedMeshRenderer>() == null)
        {
            GetComponentInChildren<MeshRenderer>().gameObject.layer = 18;
        }
        else
        {
            GetComponentInChildren<SkinnedMeshRenderer>().gameObject.layer = 18;
        }
        transform.localEulerAngles = angleHand;
        Inventory.Instance.AddSpecialItem(this);
    }
    
    

}
