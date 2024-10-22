using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public string itemName;
    public GameObject objectPrefab, uiElement;
    private bool _canInteract;
    protected bool canUse;
    public string uiText;
    public ItemCategory category;
    public bool canShowText, canInspectItem, canInteractWithItem;

    public Vector3

        angleHand; public virtual void OnGrabItem()
    {
        Inventory.Instance.AddItem(this, category);
        GetComponentInChildren<MeshRenderer>().gameObject.layer = 18;
    }

    public virtual void OnUpdate()
    {
        
    }

    public virtual void FocusObject()
    {
        
    }

    public virtual void OnInteractItem()
    {
        OnGrabItem();
    }

    public virtual void OnSelectItem()
    {
        CanInspectItem();
    }

    public virtual void OnDeselectItem()
    {
        CanvasManager.Instance.inspectImage.SetActive(false);
    }

    public virtual void OnDropItem()
    {
        gameObject.SetActive(true);
        GetComponentInChildren<MeshRenderer>().gameObject.layer = 1;
    }

    public virtual void OnInteract(bool hit, RaycastHit i)
    {
        canUse = false;
    }

    public void OnInteractWithObject()
    {
        
    }

    public string ShowText()
    {
        return uiText;
    }

    public virtual bool CanShowText()
    {
        return canShowText;
    }

    public virtual bool CanInpectItem()
    {
        return canInspectItem;
    }

    public virtual bool CanInteractWithItem()
    {
        return canInteractWithItem;
    }

    protected void CanInspectItem()
    {
        CanvasManager.Instance.inspectImage.SetActive(canInspectItem);
    }

    public virtual void ChangeCrossHair()
    {
        if(canInteractWithItem) CanvasManager.Instance.crossHairUI.IncreaseUI();
        else CanvasManager.Instance.crossHairUI.DecreaseUI();
    }
}
