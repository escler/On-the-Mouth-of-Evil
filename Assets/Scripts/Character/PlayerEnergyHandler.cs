using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEnergyHandler : MonoBehaviour
{
    public int maxAmount;
    private int _actualAmount;
    public int ActualAmount => _actualAmount;
    public Action OnEnergyChange;

    private void Awake()
    {
        _actualAmount = 100;
    }

    public void ModifiedEnergy(int amount)
    {
        _actualAmount = Mathf.Clamp(_actualAmount += amount, 0, maxAmount);
        OnEnergyChange?.Invoke();
    }

    public bool HaveEnoughEnergy()
    {
        return _actualAmount >= maxAmount;
    }
}
