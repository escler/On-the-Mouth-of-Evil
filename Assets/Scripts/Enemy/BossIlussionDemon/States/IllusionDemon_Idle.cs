using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemon_Idle : State
{
    private IllusionDemon _d;
    private float _actualTimer;
    private readonly float _timer = 2f;
    public IllusionDemon_Idle(EnemySteeringAgent e)
    {
        _d = e.GetComponent<IllusionDemon>();
    }
    
    public override void OnEnter()
    {
        _d.Anim.moving = false;
        _actualTimer = _timer;
    }

    public override void OnUpdate()
    {
        _actualTimer -= Time.deltaTime;
        if (_actualTimer > 0) return;
        
        _d.ChangeToMove();
    }

    public override void OnExit()
    {
    }
}
