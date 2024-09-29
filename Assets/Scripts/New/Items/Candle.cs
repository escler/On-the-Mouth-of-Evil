using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : Item, IInteractable
{
    private Rigidbody heldObjRb;
    private GameObject heldObj;
    public bool canTake;

    private PlayerHandler _player;

    private void Awake()
    {
        _player = PlayerHandler.Instance;
    }

    public void PickUpObject(GameObject pickUpObj)
    {
        if (!canTake) return;
        if (!pickUpObj.GetComponent<Rigidbody>()) return;
        heldObj = pickUpObj;
        heldObjRb = pickUpObj.GetComponent<Rigidbody>();
        heldObjRb.isKinematic = true;
        heldObjRb.transform.parent = _player.puzzlePivot;
        heldObj.transform.localScale = Vector3.one;
        heldObj.layer = 2;
  
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), _player.GetComponent<Collider>(), true);
    }

    public override void OnSelectItem()
    {
        if (!RitualManager.Instance) return;
        RitualManager.Instance.TakeCandle(this);
        RitualManager.Instance.candleTaked = true;
    }

    public override void OnDeselectItem()
    {
        if (!RitualManager.Instance) return;
        RitualManager.Instance.UnassignCandle();
        RitualManager.Instance.candleTaked = false;
    }

    public override void OnInteractItem()
    {
        if(!canTake) return;
        base.OnInteractItem();
        if (!RitualManager.Instance) return;
        RitualManager.Instance.TakeCandle(this);
        RitualManager.Instance.candleTaked = true;
    }

    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (!hit) return;
        print(i.transform.gameObject.name);
        if (i.transform.TryGetComponent(out RitualFloor ritualFloor))
        {
            ritualFloor.OnInteractItem();
        }
    }

    public string ShowText()
    {
        return canTake ? "Press E To Grab Candle" : "";
    }
}
