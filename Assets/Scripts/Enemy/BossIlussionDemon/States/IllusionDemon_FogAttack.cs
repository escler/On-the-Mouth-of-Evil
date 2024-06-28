using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemon_FogAttack : State
{
    private IllusionDemon _d;
    private Vector3 _position;
    private Quaternion _rotation;
    private float _actualTime;
    private bool _bossMoved;
    public IllusionDemon_FogAttack(EnemySteeringAgent e)
    {
        _d = e.GetComponent<IllusionDemon>();
    }
    public override void OnEnter()
    {
        _actualTime = 2f;
        _position = _d.transform.position;
        _rotation = _d.transform.rotation;
        _d.transform.position = new Vector3(1000, _d.transform.position.y, 1000);
        _d.actualCopies = _d.copiesPerAttack;
        Player.Instance.sphere.SetActive(true);
        _d.StartFogAttack();
    }

    public override void OnUpdate()
    {
        if (_d.actualCopies > 0) return;
        if(!_bossMoved)
        {
            _d.transform.position = _d.MoveBoss();
            _d.transform.rotation = _rotation;
            _bossMoved = true;
        }
        
        Player.Instance.sphere.GetComponent<FogPlayer>().start = false;

        
        if (_d.Anim.run) _d.transform.position += _d.transform.forward * (_d.speedRun * Time.deltaTime);
        if (Vector3.Distance(_d.CharacterPos.position, _d.transform.position) < _d.rangeForSpecialAttack)
        {
            
            _d.Anim.run = false;
            _d.Anim.jumpAttack = true;
        }
        else
        {
            _d.transform.LookAt(_d.CharacterPos);
            _d.Anim.run = true;
        }
        
        if(_d.Anim.Animator.GetCurrentAnimatorStateInfo(0).IsName("BossJumpAttack") && 
           _d.Anim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .8f)
        {
            _d.ChangeToIdle();
        }
    }

    public override void OnExit()
    {
        _d.transform.position = _d.MoveBoss();
        _d.transform.rotation = _rotation;
        _d.firstPhase = false;
        _d.secondPhase = false;
        _d.EndFogAttack();
        _d.Anim.cast = false;
        _bossMoved = false;
        Player.Instance.sphere.SetActive(false);
        
    }
}
