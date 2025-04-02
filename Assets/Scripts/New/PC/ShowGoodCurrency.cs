using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowGoodCurrency : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        ChangeText();
        GoodEssencesHandler.Instance.OnUpdateCurrency += ChangeText;
    }

    private void OnDisable()
    {
        GoodEssencesHandler.Instance.OnUpdateCurrency -= ChangeText;
    }

    private void ChangeText()
    {
        _text.text = GoodEssencesHandler.Instance.CurrentAmount.ToString();
    }
}
