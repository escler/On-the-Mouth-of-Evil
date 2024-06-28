using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemon_Moving : State
{
    private IllusionDemon _d;
    private float _yDirection;
    private float _speed, _actualTimer;
    
    public IllusionDemon_Moving(EnemySteeringAgent e)
    {
        _d = e.GetComponent<IllusionDemon>();
    }
    public override void OnEnter()
    {
        CalculateDirection();
        _d.Anim.moving = true;
        _speed = _d.speedWalk;
        var randomNum = Random.Range(1, 3);
        _actualTimer = 1 + randomNum;
    }


    public override void OnUpdate()
    {
        _d.EnemyIsMoving();
        _d.transform.LookAt(new Vector3(_d.CharacterPos.position.x, _d.transform.position.y, _d.CharacterPos.position.z));
        _d.transform.position += _d.transform.right * (_yDirection * _speed * Time.deltaTime);

        _actualTimer -= Time.deltaTime;

        if (_actualTimer <= 0)
        {
            _d.DecisionTree.Execute(_d);
        }
    }

    public override void OnExit()
    {
        _d.Anim.moving = false;
    }
    
    private void CalculateDirection()
    {
        _yDirection = Random.Range(-1, 2);
        if(_yDirection == 0) CalculateDirection();
        _d.Anim.yAxis = _yDirection;
    }

}
