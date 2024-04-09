using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    private Deadens _d;
    public Idle(Deadens d)
    {
        _d = d;
    }
    public override void OnEnter()
    {
    }

    public override void OnUpdate()
    {
        if (_d.CdForAttack > 0)
        {
            _d.CdForAttack -= Time.deltaTime;
            return;
        }
        
        _d.InFieldOfView();
        if(_d.PlayerInFov) _d.DecisionTree.Execute(_d);
    }

    public override void OnExit()
    {
        
    }
}
