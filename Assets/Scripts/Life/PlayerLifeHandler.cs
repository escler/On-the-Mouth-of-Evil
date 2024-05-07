using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeHandler : LifeHandler
{
    public delegate void UpdateLifeBar();

    public event UpdateLifeBar OnLifeChange;
    
    public override void OnTakeDamage(int damage)
    {
        _actualLife -= damage;
        print(_actualLife);

        OnLifeChange?.Invoke();
        
        if (_actualLife > 0) return;
        
        GameManager.Instance.GameLose();

    }
}
