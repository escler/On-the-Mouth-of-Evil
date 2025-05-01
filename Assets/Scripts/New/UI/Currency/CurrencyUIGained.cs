using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyUIGained : MonoBehaviour
{
    protected bool isDone;
    public bool IsDone => isDone;

    public virtual void GainCurrency(int amount)
    {
        
    }
}
