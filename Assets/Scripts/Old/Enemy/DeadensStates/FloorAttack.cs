using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorAttack : State
{
    private Deadens _d;
    private float _durationAnim;
    
    public FloorAttack(Deadens d)
    {
        _d = d;
    }
    public override void OnEnter()
    {
        _d.mageAnim.floorAttack = true;
    }

    public override void OnUpdate()
    {
        _durationAnim = _d.mageAnim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (_durationAnim >= .8f)
        {
            _d.DecisionTree.Execute(_d);
        }
    }

    public override void OnExit()
    {
        _d.mageAnim.floorAttack = false;
    }
}
