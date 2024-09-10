using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cross : Item
{
    private float _currentTime;
    public float neededTime, coolDown;
    public override void OnInteract(bool hit, RaycastHit i)
    {
        base.OnInteract(hit, i);
        if (Input.GetButton("Interact"))
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= neededTime)
            {
                print("Use la cruz");
                _currentTime = 0;
            }
        }

        if (Input.GetButtonUp("Interact")) _currentTime = 0;
    }

}
