using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : State
{
    private Deadens _d;
    private float xDirection, yDirection, _timeToSwitch;
    public Moving(Deadens d)
    {
        _d = d;
    }
    public override void OnEnter()
    {
        CalculateDirection();
        _timeToSwitch = Random.Range(1, 4);
        _d.mageAnim.moving = true;
    }

    public override void OnUpdate()
    {
        _d.mageAnim._xAxis = xDirection;
        _d.mageAnim._yAxis = yDirection;

        if (_d.CheckCollider())
        {
            xDirection *= -1;
            yDirection *= -1;
        }
            
        _d.transform.position += _d.transform.forward * (xDirection * 0.4f * Time.deltaTime) + _d.transform.right * (yDirection * 0.4f * Time.deltaTime);
        
        
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
