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

    public override void OnSelectItem()
    {
        base.OnSelectItem();
        if (!RitualManager.Instance) return;
        RitualManager.Instance.TakeCandle(this);
        RitualManager.Instance.candleTaked = true;
    }

    public override void OnDeselectItem()
    {
        base.OnDeselectItem();
        if (!RitualManager.Instance) return;
        RitualManager.Instance.UnassignCandle();
        RitualManager.Instance.candleTaked = false;
    }

    public override void OnInteractItem()
    {
        if(!canTake) return;
        base.OnInteractItem();
        if (!RitualManager.Instance) return;
        RitualManager.Instance.candleTaked = true;
    }

    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (!hit) return;
        if (i.transform.TryGetComponent(out RitualFloor ritualFloor))
        {
            ritualFloor.OnInteractItem();
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();
        canInteractWithItem = CanInteractWithItem();
        ChangeCrossHair();
        
        if(Input.GetMouseButtonDown(0)) OnInteract(rayConnected,ray);
    }
    
    public override bool CanInteractWithItem()
    {
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();

        if (!rayConnected) return false;
        if (ray.transform.TryGetComponent(out RitualFloor item)) return true;

        return false;
    }

    public string ShowText()
    {
        return canTake ? "Press E To Grab Candle" : "";
    }
}
