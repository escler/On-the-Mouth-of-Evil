using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BibleCD : MonoBehaviour
{
    public bool cantUse;
    private float _cooldown;
    public delegate void UpdateBibleCD();
    public event UpdateBibleCD OnBibleTimerChange;

    public float Cooldown => _cooldown;

    private void Awake()
    {
        _cooldown = 10;
    }

    public void SetCooldown(float time)
    {
        _cooldown = time;
        cantUse = true;
    }

    private void Update()
    {
        if (!cantUse) return;

        if (_cooldown >= 10)
        {
            _cooldown = 10;
            OnBibleTimerChange?.Invoke();
            cantUse = false;
            return;
        }
        OnBibleTimerChange?.Invoke();
        _cooldown += Time.deltaTime;
    }
}
