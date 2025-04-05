using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongboxHandle : MonoBehaviour, IInteractable
{
    public void OnInteractItem()
    {
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public void OnInteractWithObject()
    {
        StrongboxPuzzle.Instance.CheckCode();
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
