using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerEnable : MonoBehaviour
{
    TimerHandler _timerHandler;
    [SerializeField] private float timeToActivate;
    [SerializeField] private MonoBehaviour script;
    private void Start()
    {
        _timerHandler = TimerHandler.Instance;
    }

    private void Update()
    {
        if (timeToActivate > _timerHandler.Timer) return;

        print("Me active");
        script.enabled = true;
        enabled = false;
    }
}
