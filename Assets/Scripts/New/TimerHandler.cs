using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerHandler : MonoBehaviour
{
    public static TimerHandler Instance { get; private set; }
    
    private float _timer;
    public float Timer => _timer;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
    }
}
