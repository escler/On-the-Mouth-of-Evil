using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDuplication_Move : State
{
    private IllusionDuplication _i;
    private float _speed, _actualTimer, _yDirection;
    public BossDuplication_Move(EnemySteeringAgent e)
    {
        _i = e.GetComponent<IllusionDuplication>();
    }
    public override void OnEnter()
    {
        CalculateDirection();
        _i.Anim.moving = true;
        _speed = _i.speedWalk;
        _actualTimer = 5;
    }


    public override void OnUpdate()
    {
        _i.transform.LookAt(new Vector3(_i.CharacterPos.position.x, _i.transform.position.y, _i.CharacterPos.position.z));
        _i.transform.position += _i.transform.right * (_yDirection * _speed * Time.deltaTime);

        _actualTimer -= Time.deltaTime;

        if (_actualTimer <= 0)
        {
            _i.DecisionTree.Execute(_i);
        }
    }

    public override void OnExit()
    {
        _i.Anim.moving = false;
    }
    
    private void CalculateDirection()
    {
        _yDirection = Random.Range(-1, 2);
        if(_yDirection == 0) CalculateDirection();
        _i.Anim.yAxis = _yDirection;
    }

}
