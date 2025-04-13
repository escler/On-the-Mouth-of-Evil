using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTV : MonoBehaviour, IInteractable, IInteractObject
{
    [SerializeField] private Television tv;
    private bool _on;

    public void OnInteractItem()
    {

    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public void OnInteractWithObject()
    {
        _on = !_on;

        if (_on)
        {
            tv.TvOn();
        }
        else
        {
            tv.TVOff();
        }
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
