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
        InitParams();
        _infectionHandler.OnUpdateInfection += UpdateSlider;
    }

    private void OnDestroy()
    {
        _infectionHandler.OnUpdateInfection -= UpdateSlider;
    }

    private void InitParams()
    {
        _infectionHandler = InfectionHandler.Instance;
        if (_infectionHandler == null) return;
        _slider.maxValue = _infectionHandler.MaxInfection;
        _slider.gameObject.SetActive(false);
    }

    private void UpdateSlider()
    {
        _slider.gameObject.SetActive(_infectionHandler.ActualInfection > 0);

        _slider.value = _infectionHandler.ActualInfection;
    }
}
