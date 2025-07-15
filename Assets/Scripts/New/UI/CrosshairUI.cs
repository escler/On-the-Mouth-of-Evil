using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairUI : MonoBehaviour
{
    private RectTransform _transform;
    private Vector3 _initialScale;

    private void Start()
    {
        _transform = GetComponent<RectTransform>();
        _initialScale = _transform.localScale;
    }

    public void IncreaseUI()
    {
        _transform.localScale = Vector3.Lerp(_transform.localScale, _initialScale * 3, .2f);
    }

    public void DecreaseUI()
    {
        _transform.localScale = Vector3.Lerp(_transform.localScale, _initialScale, .2f);
    }
}
