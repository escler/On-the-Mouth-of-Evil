using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Light _light;

    public void OnInteractItem()
    {
        _light.enabled = !_light.enabled;
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
        return true;
    }
}
