using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemon_Hit : State
{
    private IllusionDemon _d;
    public IllusionDemon_Hit(EnemySteeringAgent e)
    {
        _d = e.GetComponent<IllusionDemon>();
    }


    public override void OnEnter()
    {
        _d.Anim.hit = true;
        if (_d.copy1.activeInHierarchy) _d.copy1.SetActive(false);
        if (_d.copy2.activeInHierarchy) _d.copy2.SetActive(false);
    }

    public override void OnUpdate()
    {
        if(_d.Anim.Animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") && 
           _d.Anim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .8f)
        {
            _d.DecisionTree.Execute(_d);
        }
    }

    public override void OnExit()
    {
        _d.Anim.hit = false;
        _d.enemyHit = false;
        _d.hitCount = 0;
    }
}
