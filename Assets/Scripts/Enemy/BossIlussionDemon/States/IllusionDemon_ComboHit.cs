using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemon_ComboHit : State
{
    private IllusionDemon _d;
    public IllusionDemon_ComboHit(EnemySteeringAgent e)
    {
        _d = e.GetComponent<IllusionDemon>();
    }
    public override void OnEnter()
    {
        _d.Anim.run = true;
    }

    public override void OnUpdate()
    {
        _d.transform.LookAt(new Vector3(_d.CharacterPos.position.x, _d.transform.position.y, _d.CharacterPos.position.z));
        if(_d.Anim.run) _d.transform.position += _d.transform.forward * (_d.speedRun * Time.deltaTime);
        if (Vector3.Distance(_d.CharacterPos.position, _d.transform.position) < _d.rangeForAttack)
        {
            _d.Anim.run = false;
            _d.Anim.comboHit = true;
        }
        
        if(_d.Anim.Animator.GetCurrentAnimatorStateInfo(0).IsName("ComboHitAttack") && 
           _d.Anim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .8f)
        {
            _d.DecisionTree.Execute(_d);
        }
    }

    public override void OnExit()
    {
        _d.Anim.comboHit = false;
        _d.Anim.run = false;
    }
}
