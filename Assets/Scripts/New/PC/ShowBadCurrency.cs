using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowBadCurrency : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        ChangeText();
        BadEssencesHandler.Instance.OnUpdateCurrency += ChangeText;
    }

    private void OnDisable()
    {
        BadEssencesHandler.Instance.OnUpdateCurrency -= ChangeText;
    }

    private void ChangeText()
    {
        _text.text = BadEssencesHandler.Instance.CurrentAmount.ToString();
    }
}
