using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossCD : MonoBehaviour
{
    public bool cantUse;
    private float _cooldown;
    public delegate void UpdateCrossUI();
    public event UpdateCrossUI OnCrossTimerChange;

    public float Cooldown => _cooldown;

    public void SetCooldown(float time)
    {
        _cooldown = time;
        cantUse = true;
    }

    private void Update()
    {
        if (!cantUse) return;

        if (_cooldown >= 30)
        {
            _cooldown = 30;
            OnCrossTimerChange?.Invoke();
            cantUse = false;
            return;
        }
        OnCrossTimerChange?.Invoke();
        _cooldown += Time.deltaTime;
    }
}
