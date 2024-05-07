using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeHandler : MonoBehaviour
{
    public int initialLife;
    protected int _actualLife;

    public int ActualLife => _actualLife;

    private void Awake()
    {
        _actualLife = initialLife;
    }

    public virtual void OnTakeDamage(int damage)
    {
        _actualLife -= damage;

        if (_actualLife > 0) return;
    }
}
