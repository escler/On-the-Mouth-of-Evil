using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemon_Banish : State
{
    private IllusionDemon _d;
    private float _actualTimer;
    
    public IllusionDemon_Banish(EnemySteeringAgent e)
    {
        _d = e.GetComponent<IllusionDemon>();
    }
    public override void OnEnter()
    {
        _d.banishPS.SetActive(true);
        _d.canBanish = true;
        _actualTimer = _d.timeToBanish;
    }

    public override void OnUpdate()
    {
        if (!_d.onBanishing) _actualTimer -= Time.deltaTime;

        if (_actualTimer > 0) return;
        
        _d.RestoreLife();
        _d.ChangeToIdle();

    }

    public override void OnExit()
    {
        _d.banishPS.SetActive(false);
        _d.canBanish = false;
    }
}
