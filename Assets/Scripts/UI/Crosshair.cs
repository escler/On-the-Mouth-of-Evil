using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    private Image _crossHair;
    public Sprite aim, noAim;

    private void Awake()
    {
        _crossHair = GetComponent<Image>();
    }

    public void OnAim()
    {
        _crossHair.sprite = aim;
    }
    
    public void OffAim()
    {
        _crossHair.sprite = noAim;

    }
}
