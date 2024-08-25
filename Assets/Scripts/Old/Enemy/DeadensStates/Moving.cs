using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : State
{
    private Deadens _d;
    private float xDirection, yDirection, _timeToSwitch, _speed;
    private bool _dirChange;
    private float _waitForChange;
    public Moving(Deadens d)
    {
        _d = d;
    }
    public override void OnEnter()
    {
        CalculateDirection();
        _speed = 0.4f;
        _timeToSwitch = Random.Range(1, 4);
        _d.mageAnim.moving = true;
        _dirChange = false;
        _waitForChange = 0;
    }

    public override void OnUpdate()
    {
        _d.mageAnim._xAxis = xDirection;
        _d.mageAnim._yAxis = yDirection;

        if (_d.CheckCollider() && !_dirChange)
        {
            xDirection *= -1;
            yDirection *= -1;
            _dirChange = true;
        }

        if (_dirChange)
        {
            _waitForChange -= Time.deltaTime;

            if (_waitForChange <= 0)
            {
                _dirChange = false;
                _waitForChange = 1f;
            }
        }
            
        _d.transform.position += _d.transform.forward * (xDirection * _speed * Time.deltaTime) + _d.transform.right * (yDirection * _speed * Time.deltaTime);
        
        
        _timeToSwitch -= Time.deltaTime;

        if (_timeToSwitch < 0)
        {
            _d.DecisionTree.Execute(_d);
        }
    }

    public override void OnExit()
    {
        _d.mageAnim.moving = false;
    }

    void CalculateDirection()
    {
        xDirection = Random.Range(-1, 2);
        yDirection = Random.Range(-1, 2);
        
        if(xDirection == 0 && yDirection == 0) CalculateDirection();
    }
}
