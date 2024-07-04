using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    private Deadens _d;
    private float _timeToSwitch;
    public Idle(EnemySteeringAgent d)
    {
        _d = d.GetComponent<Deadens>();
    }
    public override void OnEnter()
    {
        _timeToSwitch = Random.Range(.5f, 1.5f);
    }

    public override void OnUpdate()
    {
        _timeToSwitch -= Time.deltaTime;

        if (_timeToSwitch <= 0) _d.DecisionTree.Execute(_d);
    }

    public override void OnExit()
    {
        
    }
}
