using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour, IInteractable
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

    public void OnInteractItem()
    {
        if (RitualManager.Instance.candleTaked) return;
        PickUpObject(gameObject);
        RitualManager.Instance.TakeCandle(this);
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public string ShowText()
    {
        return "Press E To Grab Candle";
    }
}
