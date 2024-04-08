using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeHandler : MonoBehaviour
{
    public int initialLife;
    private int _actualLife;

    private void Awake()
    {
        _actualLife = initialLife;
    }

    public void OnTakeDamage(int damage)
    {
        _actualLife -= damage;

        if (_actualLife > 0) return;
        
        //Se Muere
    }
}
