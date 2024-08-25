using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : State
{
    private Deadens _d;
    public Chase(Deadens d)
    {
        _d = d;
    }
    public override void OnEnter()
    {
        //_d.Animator.SetBool("Walk", true);
    }

    public override void OnUpdate()
    {
        _d.Arrive();
        
        if(_d.InRangeForAttack) _d.DecisionTree.Execute(_d);
    }

    public override void OnExit()
    {
    }
}
