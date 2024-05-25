using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemon_Hit : State
{
    private IllusionDemon d;
    private float _durationAnim;
    public IllusionDemon_Hit(EnemySteeringAgent e)
    {
        d = e.GetComponent<IllusionDemon>();
    }


    public override void OnEnter()
    {
        d.Anim.hit = true;
    }

    public override void OnUpdate()
    {
        _durationAnim = d.Anim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (_durationAnim >= .8f)
        {
            d.ChangeToMove();
        }
    }

    public override void OnExit()
    {
        d.Anim.hit = false;
        d.enemyHit = false;
    }
}
