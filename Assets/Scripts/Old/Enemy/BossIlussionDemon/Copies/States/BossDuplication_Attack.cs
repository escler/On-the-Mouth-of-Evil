using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDuplication_Attack : State
{
    private IllusionDuplication _i;
    
    public BossDuplication_Attack(EnemySteeringAgent e)
    {
        _i = e.GetComponent<IllusionDuplication>();
    }
    public override void OnEnter()
    {
        _i.Anim.run = true;
    }

    public override void OnUpdate()
    {
        _i.transform.LookAt(new Vector3(_i.CharacterPos.position.x, _i.transform.position.y, _i.CharacterPos.position.z));
        if(_i.Anim.run) _i.transform.position += _i.transform.forward * (_i.speedRun * Time.deltaTime);
        if (Vector3.Distance(_i.CharacterPos.position, _i.transform.position) < _i.rangeForAttack)
        {
            _i.Anim.run = false;
            _i.Anim.attack = true;
        }
        
        if(_i.Anim.Animator.GetCurrentAnimatorStateInfo(0).IsName("ComboHitAttack") && 
           _i.Anim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .8f)
        {
            _i.DecisionTree.Execute(_i);
        }
    }

    public override void OnExit()
    {
        _i.Anim.attack = false;
        _i.Anim.run = false;
    }
}
