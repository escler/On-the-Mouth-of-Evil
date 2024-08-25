using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : State
{
    private Deadens _d;
    private float _durationAnim;
    public Hit(Deadens d)
    {
        _d = d;
    }
    public override void OnEnter()
    {
        _d.mageAnim.hit = true;
        _d.canHit = false;
        _d.waitForHitAgain = 5f;
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
        _d.enemyHit = false;
        _d.mageAnim.hit = false;
    }
}
