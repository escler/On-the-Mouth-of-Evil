using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerEnable : MonoBehaviour
{
    TimerHandler _timerHandler;
    [SerializeField] private float timeToActivate;
    [SerializeField] private MonoBehaviour script;
    [SerializeField] private string mission;
    private void Start()
    {
        _timerHandler = TimerHandler.Instance;
        if (PlayerPrefs.GetInt(mission) == 1) timeToActivate /= 2;
        print(timeToActivate);
    }

    private void Update()
    {
        if (timeToActivate > _timerHandler.Timer) return;

        print("Me active");
        script.enabled = true;
        enabled = false;
    }
}
