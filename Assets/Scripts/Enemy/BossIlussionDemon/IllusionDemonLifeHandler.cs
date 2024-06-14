using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemonLifeHandler : LifeHandler
{
    public Action OnLifeChange;
    public override void OnTakeDamage(int damage)
    {
        _actualLife -= damage;
        OnLifeChange?.Invoke();
        var illusionDemon = GetComponent<IllusionDemon>();
        if (_actualLife > 0)
        {
            if (illusionDemon.canHit)
            {
                illusionDemon.hitCount++;
            }
            return;
        }
        illusionDemon.canHit = false;
        illusionDemon.canBanish = true;
        illusionDemon.ChangeToBanish();
    }

    public void RechargeLife()
    {
        _actualLife = initialLife / 2;
        GetComponent<IllusionDemon>().ChangeToIdle();
        OnLifeChange?.Invoke();
    }

    public void Death()
    {
        var illusionDemon = GetComponent<IllusionDemon>();
        illusionDemon.Anim.death = true;
        illusionDemon.GetComponentInChildren<CapsuleCollider>().enabled = false;
        illusionDemon.enabled = false;
    }
}
