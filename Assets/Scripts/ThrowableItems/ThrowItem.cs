using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItem : MonoBehaviour
{
    private Vector3 _location;
    public bool _moving;
    Vector3 zero = Vector3.zero;
    private float _time = 1f;
    public float speedRot;

    public void Update()
    {
        transform.Rotate(0,speedRot * Time.deltaTime,0);
        if (!_moving) return;

        if (Vector3.Distance(transform.position, _location) <= .5) _moving = false;
        transform.position = Vector3.SmoothDamp(transform.position, _location, ref zero, _time);
    }

    public void SetLocation(Vector3 location)
    {
        _location = location;
        _moving = true;
    }
}
