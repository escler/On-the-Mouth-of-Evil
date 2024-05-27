using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemon_Cast : State
{
    private IllusionDemon _d;
    public IllusionDemon_Cast(EnemySteeringAgent e)
    {
        _d = e.GetComponent<IllusionDemon>();
    }
    public override void OnEnter()
    {
        _d.Anim.cast = true;
    }

    public override void OnUpdate()
    {
    }

    public override void OnExit()
    {
    }
}
