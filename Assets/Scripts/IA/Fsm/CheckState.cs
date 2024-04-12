using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class CheckState : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    
    public void ChangeCurrentStateText(string state)
    {
        _text.text = state;
    }
    
    
}
