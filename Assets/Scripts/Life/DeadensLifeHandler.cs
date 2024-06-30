using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadensLifeHandler : LifeHandler
{
    public override void OnTakeDamage(int damage)
    {
        base.OnTakeDamage(damage);
        var enemy = GetComponentInParent<DemonLowRange>();
        if (_actualLife > 0) return;

        enemy.canBanish = true;
    }
}
