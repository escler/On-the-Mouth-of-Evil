using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IllusionDemonLifeHandler : LifeHandler
{
    public Action OnLifeChange;
    private bool _firstCalled, _secondcalled;
    public float percent70, percent30;

    private void Awake()
    {
        percent70 = initialLife * 0.7f;
        percent30 = initialLife * 0.3f;
    }

    public override void TakeDamage(int damage, int force)
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

            if (_actualLife < percent70 && !_firstCalled)
            {
                illusionDemon.firstPhase = true;
                _firstCalled = true;
            }

            if (_actualLife < percent30 && !_secondcalled)
            {
                illusionDemon.secondPhase = true;
                _secondcalled = true;
            }
            return;
        }
        illusionDemon.canHit = false;
        illusionDemon.canBanish = true;
    }

    public void RechargeLife()
    {
        _actualLife = initialLife / 2;
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
