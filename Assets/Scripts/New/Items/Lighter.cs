using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighter : Item
{
    [SerializeField] private List<Item> _itemsInteractables;
    public GameObject PSIdle;
    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (!hit) return;
        if (i.transform.TryGetComponent(out IBurneable item))
        {
            item.OnBurn();
            canUse = true;
        }
        else
        {
            canUse = false;
        }
        base.OnInteract(hit,i);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();
        canInteractWithItem = CanInteractWithItem();
        ChangeCrossHair();
        if(Input.GetMouseButtonDown(0)) OnInteract(rayConnected, ray);
    }

    public override bool CanInteractWithItem()
    {
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();

        if (!rayConnected) return false;
        if (ray.transform.TryGetComponent(out IBurneable item)) return true;

        return false;
    }

    public override void OnGrabItem()
    {
        base.OnGrabItem();
        transform.localEulerAngles = angleHand;
    }

    public override void OnSelectItem()
    {
        base.OnSelectItem();
        PSIdle.SetActive(true);
    }

    public override void OnDropItem()
    {
        GetComponentInChildren<MeshRenderer>().gameObject.layer = 1;
        PSIdle.SetActive(false);
        gameObject.SetActive(true);
    }
}
