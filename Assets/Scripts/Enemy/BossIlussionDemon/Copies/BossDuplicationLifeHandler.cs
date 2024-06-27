using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDuplicationLifeHandler : LifeHandler
{
    public override void OnTakeDamage(int damage)
    {
        base.OnTakeDamage(damage);

        if (_actualLife > 0) return;

        GetComponentInChildren<DissolveEnemy>().ActivateDissolve();
    }
}
