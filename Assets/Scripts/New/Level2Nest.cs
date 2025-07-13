using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Nest : MonoBehaviour
{
    private bool _dialogStarted;

    private void OnTriggerEnter(Collider other)
    {
        if (_dialogStarted) return;
        
        DialogHandler.Instance.ChangeText("This seems to be the core of the demon’s influence… its nests grow here.");
        _dialogStarted = true;
    }
}
