using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemon_Moving : State
{
    private IllusionDemon d;
    private float _yDirection;
    private float _speed, _actualTimer;
    
    public IllusionDemon_Moving(EnemySteeringAgent e)
    {
        d = e.GetComponent<IllusionDemon>();
    }
    public override void OnEnter()
    {
        CalculateDirection();
        d.Anim.moving = true;
        _speed = d.speedWalk;
        _actualTimer = 5;

    }


    public override void OnUpdate()
    {
        d.transform.LookAt(new Vector3(d.CharacterPos.position.x, d.transform.position.y, d.CharacterPos.position.z));
        d.transform.position += d.transform.right * (_yDirection * _speed * Time.deltaTime);

        _actualTimer -= Time.deltaTime;

        if (_actualTimer <= 0)
        {
            d.ChangeToCombo();
            CalculateDirection();
            _actualTimer = 5;
        }
    }

    public override void OnExit()
    {
    }
    
    private void CalculateDirection()
    {
        _yDirection = Random.Range(-1, 2);
        if(_yDirection == 0) CalculateDirection();
        d.Anim.yAxis = _yDirection;
    }

}
