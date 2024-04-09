using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    private Deadens _d;
    
    public Attack(Deadens d)
    {
        _d = d;
    }
    
    public override void OnEnter()
    {
        _d.Attack();
        _d.CdForAttack = _d.initialCdForAttack;
        _d.DecisionTree.Execute(_d);
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
    }
}
