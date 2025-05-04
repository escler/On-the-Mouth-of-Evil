using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    private string _text;

    private void Awake()
    {
        _text = "Go To The Confessional";
        CompleteTask();
    }

    public void ChangeText(string text)
    {
        tmp.color = Color.white;
        _text = text;
        tmp.text = _text;
    }

    public void CompleteTask()
    {
        tmp.text = _text;
        tmp.color = Color.green;
        tmp.text = "<s>" + _text + "</s>";
    }
}
