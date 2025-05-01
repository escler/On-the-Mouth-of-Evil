using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoodEssenceUI : CurrencyUIGained
{
    GoodEssencesHandler _currencyHandler;
    [SerializeField] private TextMeshProUGUI current, gained; 

    private void OnEnable()
    {
        _currencyHandler = GoodEssencesHandler.Instance;
        GainCurrency(DecisionsHandler.Instance.badPath ? 0 : LevelGainCurrency.Instance.goodEssence);
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
            actual += 2;
            current.text = actual.ToString();
            yield return new WaitForSeconds(0.1f);
        }
        _currencyHandler.AddCurrency(gainedAmount);
        isDone = true;
        GameManagerNew.Instance.CheckProceed();
    }
}
