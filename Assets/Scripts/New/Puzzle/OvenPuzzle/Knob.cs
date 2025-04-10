using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knob : MonoBehaviour, IInteractable
{
    public void OnInteractItem()
    {
        Oven.Instance.ChangeFireState(this);
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public void OnInteractWithObject()
    {
    }

    public string ShowText()
    {
        return "";
    }

    public bool CanShowText()
    {
        return false;
    }
}
