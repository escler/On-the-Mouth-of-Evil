using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraItem : MonoBehaviour
{
    public float timer;
    TimerHandler _timerHandler;
    [SerializeField] private GameObject ps;
    public bool onHand;
    private bool _startCheck;
    

    private void Awake()
    {
        StartCoroutine(AwakeWithDelay());
    }

    IEnumerator AwakeWithDelay()
    {
        yield return new WaitForSeconds(1f);
        _timerHandler = TimerHandler.Instance;
        _startCheck = true;
    }

    private void Update()
    {
        if (!_startCheck) return;
        ps.SetActive(_timerHandler.Timer > timer && !onHand);
    }

}
