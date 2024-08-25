using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDuplication_Explode : State
{
    private IllusionDuplication _i;
    
    public BossDuplication_Explode(EnemySteeringAgent e)
    {
        _i = e.GetComponent<IllusionDuplication>();
    }
    public override void OnEnter()
    {
    }

    public override void OnUpdate()
    {
    }

    public override void OnExit()
    {
    }
}
