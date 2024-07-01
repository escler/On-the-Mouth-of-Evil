using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadensLifeHandler : LifeHandler
{
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        var enemy = GetComponentInParent<DemonLowRange>();
        if (_actualLife > 0) return;

        enemy.canBanish = true;
    }
}
