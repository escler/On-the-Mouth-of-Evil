using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class PlayerLifeHandler : LifeHandler
{
    public delegate void UpdateLifeBar();
    public event UpdateLifeBar OnLifeChange;
    public int amountPerEnemyBanished;

    public override void OnTakeDamage(int damage)
    {
        base.OnTakeDamage(damage);

        OnLifeChange?.Invoke();
        
        if (_actualLife > 0) return;
        
        GameManager.Instance.GameLose();
    }

    public void AddHealth(int amount)
    {
        _actualLife += Mathf.Clamp(_actualLife += amount * amountPerEnemyBanished, 0, initialLife);
        OnLifeChange?.Invoke();
    }
}
