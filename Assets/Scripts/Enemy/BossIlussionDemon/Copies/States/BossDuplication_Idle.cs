using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDuplication_Idle : State
{
    private IllusionDuplications _i;
    private float _actualTimer;
    private float _timer = 3f;

    public BossDuplication_Idle(EnemySteeringAgent e)
    {
        _i = e.GetComponent<IllusionDuplications>();
    }

    public override void OnEnter()
    {
        _i.Anim.moving = false;
        _actualTimer = _timer;
    }

    public override void OnUpdate()
    {
        _actualTimer -= Time.deltaTime;
        if (_actualTimer > 0) return;
        
        _i.ChangeToMove();
    }

    public override void OnExit()
    {
    }
}
