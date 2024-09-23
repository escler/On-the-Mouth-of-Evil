using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    protected Image _image;
    
    private void OnEnable()
    {
        _image = GetComponent<Image>();
    }

    private void OnDisable()
    {
        _image.color = Color.white;
    }

}
