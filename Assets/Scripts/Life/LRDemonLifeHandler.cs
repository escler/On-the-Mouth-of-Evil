using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRDemonLifeHandler : LifeHandler
{
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        var enemy = GetComponentInParent<DemonLowRange>();
        if (_actualLife > 0)
        {
            enemy.AddHitCount();
            return;
        }

        enemy.canBanish = true;
    }
}
