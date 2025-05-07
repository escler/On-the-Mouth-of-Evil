using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : Item, IInteractable
{
    private Rigidbody heldObjRb;
    private GameObject heldObj;
    public bool canTake;
    public bool badCandle;
    public bool placedInRitual;
    [SerializeField] Material goodCandleMat, badCandleMat;
    [SerializeField] GameObject candleGoodUI, candleBadUI;
    
    private PlayerHandler _player;

    private void Awake()
    {
        _player = PlayerHandler.Instance;
        GetComponentInChildren<MeshRenderer>().material = badCandle ? badCandleMat : goodCandleMat;
        uiElement = badCandle ? candleBadUI : candleGoodUI;
    }

    public override void OnGrabItem()
    {
        base.OnGrabItem();
        transform.localEulerAngles = angleHand;
        GetComponent<BoxCollider>().isTrigger = false;
    }

    public override void OnSelectItem()
    {
        base.OnSelectItem();
        if (!RitualManager.Instance) return;
        RitualManager.Instance.candleTaked = true;

    }

    public override void OnDeselectItem()
    {
        base.OnDeselectItem();
        if (!RitualManager.Instance) return;
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
        if (i.transform.TryGetComponent(out CandleRitual candleRitual))
        {
            if (candleRitual.isCandlePlaced) return;
            if (RitualManager.Instance.firstCandlePlaced == null)
            {
                candleRitual.GetCandle(this);
            }else if (RitualManager.Instance.firstCandlePlaced.badCandle ==
                      badCandle && !candleRitual.isCandlePlaced)
            {
                candleRitual.GetCandle(this);

            }
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();
        canShowText = CanInteractWithItem();
        ChangeCrossHair();
        
        if(Input.GetButtonDown("Interact")) OnInteract(rayConnected,ray);
    }
    
    public override bool CanInteractWithItem()
    {
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();

        if (!rayConnected) return false;
        
        if (ObjectDetector.Instance.InteractText()) return true;
        if (ray.transform.TryGetComponent(out CandleRitual item))
        {
            if (RitualManager.Instance.firstCandlePlaced == null) return true;
            if (RitualManager.Instance.firstCandlePlaced.placedInRitual == badCandle && !item.isCandlePlaced)
                return true;
        }
        return false;
    }

    public string ShowText()
    {
        return canTake ? "Press E To Grab Candle" : "";
    }
}
