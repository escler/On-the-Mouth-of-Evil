using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    private Image _image;

    private void OnEnable()
    {
        _image = GetComponent<Image>();
    }

    private void OnDisable()
    {
        _image.color = Color.white;
    }
}
