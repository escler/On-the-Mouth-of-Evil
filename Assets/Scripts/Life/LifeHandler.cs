using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeHandler : MonoBehaviour
{
    public int initialLife;
    protected int _actualLife;

    public int ActualLife => _actualLife;

    private void OnEnable()
    {
        _actualLife = initialLife;
    }

    public virtual void TakeDamage(int damage)
    {
        _actualLife -= damage;
    }
}
