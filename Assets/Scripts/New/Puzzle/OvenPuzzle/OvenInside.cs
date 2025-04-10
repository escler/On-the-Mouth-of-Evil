using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenInside : MonoBehaviour, IInteractable
{
    public bool isBearInside;
    public void OnInteractItem()
    {
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
