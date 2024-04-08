using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : State
{
    private Deadens _d;
    public Chase(Deadens d)
    {
        _d = d;
    }
    public override void OnEnter()
    {
        
    }

    public override void OnUpdate()
    {
        _d.Arrive();
    }

    public override void OnExit()
    {
    }
}
