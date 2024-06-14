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

    public override void Execute(IllusionDemon i)
    {
        switch (action)
        {
            case Actions.FogAttack:
                if(i.lastActionAttack == Actions.FogAttack) i.DecisionTree.Execute(i);
                else
                {
                    i.lastActionAttack = Actions.FogAttack;
                    i.ChangeToFogAttack();
                }
                break;
            case Actions.Attack:
                if (i.lastActionAttack == Actions.Attack)
                {
                    i.DecisionTree.Execute(i);
                }
                else
                {
                    i.lastActionAttack = Actions.Attack;
                    i.lastAction = actionsEnemy.Attack;
                    i.ChangeToChannelAttack();
                }

                break;
            case Actions.SpecialAttack:
                if (i.lastActionAttack == Actions.SpecialAttack)
                {
                    i.DecisionTree.Execute(i);
                }
                else
                {
                    i.lastActionAttack = Actions.SpecialAttack;
                    i.lastAction = actionsEnemy.Attack;
                    i.ChangeToJumpAttack();
                }

                break;
            case Actions.SpecialAttack2:
                if (i.lastActionAttack == Actions.SpecialAttack2)
                {
                    i.DecisionTree.Execute(i);
                }
                else
                {
                    i.lastActionAttack = Actions.SpecialAttack2;
                    i.ChangeToThrowAttack();
                }

                break;
        }
    }

    public override void Execute(IllusionDuplication d)
    {
        switch (action)
        {
            case Actions.Move:
                d.lastAction = actionsEnemy.NotAttack;
                d.ChangeToMove();
                break;
            case Actions.Attack:
                d.lastAction = actionsEnemy.Attack;
                d.ChangeToAttack();
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
    Hit,
    SpecialAttack,
    SpecialAttack2,
    Cast,
    BossDupicationCopy,
    FogAttack
}
