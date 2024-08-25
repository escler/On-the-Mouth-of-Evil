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
        SetText();
        StartCoroutine(StartTimer());
    }

    private void OnDisable()
    {
        StopCoroutine(StartTimer());
    }

    void SetText()
    {
        _tmp.text = _startTime.ToString("F2");
    }

    IEnumerator StartTimer()
    {
        while (_startTime > 0)
        {
            _startTime -= _refreshRate;
            SetText();
            yield return new WaitForSeconds(_refreshRate);
        }
    }
}
