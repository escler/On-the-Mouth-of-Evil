using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TimerUIType : MonoBehaviour
{
    private TextMeshProUGUI _tmp;
    private float _startTime;
    private float _refreshRate = 0.1f;

    private void Awake()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        _startTime = TypeManager.Instance.timeToResolve;
        _tmp.text = _startTime.ToString();
        StartCoroutine(StartTimer());
    }

    private void OnDisable()
    {
        StopCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        while (_startTime > 0)
        {
            _startTime -= _refreshRate;
            _tmp.text = _startTime.ToString();
            yield return new WaitForSeconds(_refreshRate);
        }
    }
}
