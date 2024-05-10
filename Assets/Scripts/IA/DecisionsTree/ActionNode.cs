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
            case Actions.Hit:
                deadens.HitState();
                break;
            case Actions.Idle:
                deadens.IdleState();
                break;
            case Actions.Move:
                deadens.MovingSate();
                break;
            case Actions.Attack:
                deadens.AttackState();
                break;
            case Actions.FloorAttack:
                deadens.FloorState();
                break;
        }
    }
}

public enum Actions
{
    Idle,
    Move,
    Attack,
    FloorAttack,
    Hit
}
