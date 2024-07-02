using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeHandler : LifeHandler
{
    public delegate void UpdateLifeBar();
    public event UpdateLifeBar OnLifeChange;
    public event UpdateLifeBar OnTakeDamage;
    public int amountPerEnemyBanished;

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        OnLifeChange?.Invoke();
        OnTakeDamage?.Invoke();
        
        if (_actualLife > 0) return;
        
        GameManager.Instance.GameLose();
    }

    public void AddHealth(int amount)
    {
        _actualLife = Mathf.Clamp(_actualLife += amount * amountPerEnemyBanished, 0, initialLife);
        OnLifeChange?.Invoke();
    }
}
