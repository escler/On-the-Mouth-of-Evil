using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRDemonLifeHandler : LifeHandler
{
    public override void TakeDamage(int damage, int force, int hitCount)
    {
        base.TakeDamage(damage, force, hitCount);
        var enemy = GetComponentInParent<DemonLowRange>();
        if (_actualLife > 0)
        {
            enemy.AddHitCount(hitCount);
            enemy.force = force;
            return;
        }

        enemy.canBanish = true;
    }
}
