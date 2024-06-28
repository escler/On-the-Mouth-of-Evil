using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemon_JumpAttack : State
{
    private IllusionDemon _d;
    private float _durationAnim;
    
    public IllusionDemon_JumpAttack(EnemySteeringAgent e)
    {
        _d = e.GetComponent<IllusionDemon>();
    }
    public override void OnEnter()
    {
        _d.Anim.castCopies = true;
        _d.transform.position = _d.LocationForJumpAttack();
    }

    public override void OnUpdate()
    {
        _d.EnemyIsMoving();
        if(!_d.Anim.jumpAttack)
            _d.transform.LookAt(new Vector3(_d.CharacterPos.position.x, _d.transform.position.y, _d.CharacterPos.position.z));
        
        if(_d.finishCast)
        {
            _d.Anim.run = true;
            _d.copy1.GetComponentInChildren<BossDuplicationMovement>().ChangeRun(true);
            _d.copy2.GetComponentInChildren<BossDuplicationMovement>().ChangeRun(true);
            _d.finishCast = false;
        }

        if (_d.Anim.run) _d.transform.position += _d.transform.forward * (_d.speedRun * Time.deltaTime);
        if (Vector3.Distance(_d.CharacterPos.position, _d.transform.position) < _d.rangeForSpecialAttack)
        {
            
            _d.Anim.run = false;
            _d.Anim.jumpAttack = true;
        }
        
        if(_d.Anim.Animator.GetCurrentAnimatorStateInfo(0).IsName("BossJumpAttack") && 
           _d.Anim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .8f)
        {
            _d.ChangeToIdle();
        }
    }

    public override void OnExit()
    {
        _d.Anim.jumpAttack = false;
        _d.Anim.castCopies = false;
        _d.Anim.run = false;
        _d.finishCast = false;
    }
}
