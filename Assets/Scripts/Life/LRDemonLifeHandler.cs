using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRDemonLifeHandler : LifeHandler
{
    public override void TakeDamage(int damage, int force)
    {
        base.TakeDamage(damage, force);
        var enemy = GetComponentInParent<DemonLowRange>();
        if (_actualLife > 0)
        {
            enemy.AddHitCount();
            enemy.force = force;
            return;
        }

        enemy.canBanish = true;
    }
}
