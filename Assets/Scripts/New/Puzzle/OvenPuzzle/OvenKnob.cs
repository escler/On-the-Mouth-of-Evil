using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenKnob : MonoBehaviour, IInteractable, IInteractObject
{
    public void OnInteractItem()
    {
        
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public void OnInteractWithObject()
    {
        Oven.Instance.CheckCode();
    }

    public string ShowText()
    {
        return "";
    }

    public bool CanShowText()
    {
        return false;
    }

    public void OnInteractWithThisObject()
    {
        OnInteractWithObject();
    }
}
