using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    private Image _crossHair;

    private void Awake()
    {
        _crossHair = GetComponent<Image>();
    }

    public void TurnOn()
    {
        _crossHair.enabled = true;
    }
    
    public void TurnOff()
    {
        _crossHair.enabled = false;
    }
}
