using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfectionUI : MonoBehaviour
{
    private Slider _slider;
    InfectionHandler _infectionHandler;

    private void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
        StartCoroutine(WaitCor());
    }

    IEnumerator WaitCor()
    {
        yield return new WaitForSeconds(0.1f);
        InitParams();
    }

    private void OnDestroy()
    {
        if (_infectionHandler == null) return;
        _infectionHandler.OnUpdateInfection -= UpdateSlider;
    }

    private void InitParams()
    {
        _infectionHandler = InfectionHandler.Instance;
        _infectionHandler.OnUpdateInfection += UpdateSlider;
        _slider.maxValue = _infectionHandler.MaxInfection;
        _slider.gameObject.SetActive(false);
    }

    private void UpdateSlider()
    {
        _slider.gameObject.SetActive(_infectionHandler.ActualInfection > 0);

        _slider.value = _infectionHandler.ActualInfection;
    }
}
