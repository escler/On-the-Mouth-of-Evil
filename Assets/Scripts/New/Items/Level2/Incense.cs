using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incense : Item
{
    [SerializeField] ParticleSystem particle;
    private bool _incenseActivated;

    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (canInteractWithItem) return;
        if (_incenseActivated) return;
        var emit = particle.emission;
        emit.rateOverTime = 5;
        _incenseActivated = true;
        PlayerHandler.Instance.incenseProtect = true;
    }

    void Awake()
    {
        var emit = particle.emission;
        emit.rateOverTime = 0;
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
        SortInventoryBuyHandler.Instance.SaveCount(itemName, true);
    }

    public override void OnDropItem()
    {
        base.OnDropItem();
        SortInventoryBuyHandler.Instance.SaveCount(itemName, false);
    }
}
