using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarUI : MonoBehaviour
{
    private Slider _slider;
    private int _actualLife, _maxLife;
    private PlayerLifeHandler _lifeHandler;
    
    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponent<Slider>();
        _lifeHandler = Player.Instance.GetComponent<PlayerLifeHandler>();
        _lifeHandler.OnLifeChange += ChangeValue;
        ChangeValue();
    }

    private void OnDestroy()
    {
        _lifeHandler.OnLifeChange -= ChangeValue;
    }

    private void ChangeValue()
    {
        _slider.maxValue = _lifeHandler.initialLife;
        _slider.value = _lifeHandler.ActualLife;

    }
}
