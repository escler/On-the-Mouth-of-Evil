using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    private Deadens _d;
    private float _cdForAttack;
    
    public Attack(Deadens d)
    {
        _d = d;
    }
    
    public override void OnEnter()
    {
        _d.Animator.SetBool("Attack", true);
        _cdForAttack = 1.10f;
    }

    public override void OnUpdate()
    {
        _cdForAttack -= Time.deltaTime;
        if (_cdForAttack <= 0)
        {
            _d.Attack();
        }
    }

    public override void OnExit()
    {
    }
}
