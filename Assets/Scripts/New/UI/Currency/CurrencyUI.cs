using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CurrencyUI : CurrencyUIGained
{
    CurrencyHandler _currencyHandler;
    [SerializeField] private TextMeshProUGUI current, gained; 

    private void OnEnable()
    {
        _currencyHandler = CurrencyHandler.Instance;
        GainCurrency(LevelGainCurrency.Instance.currency);
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
        current.text = actual.ToString();
        gained.text = "+" + gainedAmount;

        while (actual < updateAmount)
        {
            actual += 1;
            current.text = actual.ToString();
            yield return new WaitForSeconds(0.05f);
        }
        _currencyHandler.AddCurrency(gainedAmount);
        isDone = true;
        GameManagerNew.Instance.CheckProceed();
    }
}
