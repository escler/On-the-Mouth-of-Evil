using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEnemyLifeHandler : LifeHandler
{
    public override void OnTakeDamage(int damage)
    {
        _actualLife -= damage;

        if (_actualLife > 0) return;
        
        GetComponent<ExplosionEnemy>().ActiveExplosion();
    }
}
