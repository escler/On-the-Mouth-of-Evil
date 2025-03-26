using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowCurrency : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        ChangeText();
        CurrencyHandler.Instance.OnUpdateCurrency += ChangeText;
    }

    private void OnDisable()
    {
        CurrencyHandler.Instance.OnUpdateCurrency -= ChangeText;

    }

    private void ChangeText()
    {
        _text.text = "$ " + CurrencyHandler.Instance.CurrentAmount;
    }
}
