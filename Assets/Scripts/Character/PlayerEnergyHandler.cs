using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergyHandler : MonoBehaviour
{
    public int maxAmount;
    private int _actualAmount;

    private void Awake()
    {
        _actualAmount = maxAmount / 2;
    }

    public void AddEnergy(int amount)
    {
        _actualAmount += amount;
        Mathf.Clamp(_actualAmount, 0, maxAmount);
        print(_actualAmount);
    }
}
