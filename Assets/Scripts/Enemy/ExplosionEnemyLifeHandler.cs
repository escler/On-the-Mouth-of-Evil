using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEnemyLifeHandler : LifeHandler
{
    public override void TakeDamage(int damage, int force)
    {
        _actualLife -= damage;

        if (_actualLife > 0) return;
        
        GetComponent<ExplosionEnemy>().ActiveExplosion();
    }
}
