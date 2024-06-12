using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemon_ThrowObjects : State
{
    private IllusionDemon _d;
    public IllusionDemon_ThrowObjects(EnemySteeringAgent e)
    {
        _d = e.GetComponent<IllusionDemon>();
    }
    public override void OnEnter()
    {
        ThrowManager.Instance.MoveToLocation(_d.throwObjectPos.position);
    }

    public override void OnUpdate()
    {
    }

    public override void OnExit()
    {
    }
}
