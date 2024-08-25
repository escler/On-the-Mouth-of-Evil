using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDuplicationLifeHandler : LifeHandler
{
    public override void TakeDamage(int damage, int force, int hitCount)
    {
        base.TakeDamage(damage, force, hitCount);

        if (_actualLife > 0) return;

        GetComponentInParent<DissolveEnemy>().ActivateDissolve();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6) GetComponentInParent<DissolveEnemy>().ActivateDissolve();
    }
}
