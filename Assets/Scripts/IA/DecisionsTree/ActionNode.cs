using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionNode : DecisionNode
{
    public Actions action;
    public override void Execute(Deadens deadens)
    {
        switch (action)
        {
            case Actions.Idle:
                deadens.IdleState();
                break;
            case Actions.Pf:
                deadens.PfState();
                break;
            case Actions.Chase:
                deadens.ChaseState();
                break;
            case Actions.Attack:
                deadens.AttackState();
                break;
        }
    }
}

public enum Actions
{
    Idle,
    Pf,
    Chase,
    Attack
}
