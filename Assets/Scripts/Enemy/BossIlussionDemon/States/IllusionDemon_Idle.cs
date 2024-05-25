using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemon_Idle : State
{
    private IllusionDemon d;
    public IllusionDemon_Idle(EnemySteeringAgent e)
    {
        d = e.GetComponent<IllusionDemon>();
    }
    
    public override void OnEnter()
    {
        d.Anim.moving = false;
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
    }
}
