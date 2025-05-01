using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RespectUI : CurrencyUIGained
{
    RespectHandler _currencyHandler;
    [SerializeField] private TextMeshProUGUI actualLevel;
    [SerializeField] Slider slider;
    int _minRespect, _maxRespect, _actualRespect, _actualLevel;

    private void OnEnable()
    {
        _currencyHandler = RespectHandler.Instance;
        LevelParams();
        slider.value = _actualRespect;
        GainCurrency(LevelGainCurrency.Instance.currency);
    }

    private void LevelParams()
    {
        _actualLevel = _currencyHandler.CurrentLevel;
        var pair = _currencyHandler.Levels[_actualLevel];
        _minRespect = pair[0];
        _maxRespect = pair[1];
        slider.minValue = _minRespect;
        slider.maxValue = _maxRespect;
        actualLevel.text = "Respect Level: " + _actualLevel;
    }

    public override void GainCurrency(int amount)
    {
        isDone = false;
        StartCoroutine(UpdateCurrency(amount));
    }

    IEnumerator UpdateCurrency(int gainedAmount)
    {
        var actual = _currencyHandler.CurrentAmount;
        var updateAmount = actual + gainedAmount;

        while (actual < updateAmount)
        {
            actual += 1;
            if (actual >= _maxRespect)
            {
                _currencyHandler.AddLevel(1);
                LevelParams();
            }
            slider.value = actual;
            yield return new WaitForSeconds(0.01f);
        }
        _currencyHandler.AddRespect(gainedAmount);
        isDone = true;
        GameManagerNew.Instance.CheckProceed();
    }
}
