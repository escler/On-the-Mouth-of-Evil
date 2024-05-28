using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemon_Idle : State
{
    private IllusionDemon _d;
    public IllusionDemon_Idle(EnemySteeringAgent e)
    {
        _d = e.GetComponent<IllusionDemon>();
    }
    
    public override void OnEnter()
    {
        _d.Anim.moving = false;
    }

    public override void OnUpdate()
    {
        _d.ChangeToMove();
    }

    public override void OnExit()
    {
    }
}
