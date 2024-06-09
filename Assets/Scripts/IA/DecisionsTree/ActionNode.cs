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
            case Actions.Hit:
                i.ChangeToHit();
                break;
            case Actions.Move:
                i.lastAction = actionsEnemy.NotAttack;
                i.ChangeToMove();
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
                    i.ChangeToCastAttack();
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
                    i.ChangeToSpecialAttack();
                }

                break;
            case Actions.Cast:
                if (i.lastActionAttack == Actions.Cast || i.enemiesCount > 0)
                {
                    i.DecisionTree.Execute(i);
                }
                else
                {
                    i.lastActionAttack = Actions.Cast;
                    i.lastAction = actionsEnemy.Attack;
                    i.ChangeCastAttack();
                }
                break;
            case Actions.BossDupicationCopy:
                if (i.fightingCopies > 0)
                {
                    i.DecisionTree.Execute(i);
                }
                else
                {
                    i.lastActionAttack = Actions.BossDupicationCopy;
                    i.lastAction = actionsEnemy.Attack;
                    i.ChangeToDuplicationFight();
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
    Cast,
    BossDupicationCopy
}
