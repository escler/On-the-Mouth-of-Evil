using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionNode : DecisionNode
{
    public DecisionNode falseNode, trueNode;

    public Questions question;

    private float randomRange;
    
    public override void Execute(Deadens deadens)
    {
        randomRange = Random.Range(1, 100);
        switch (question)
        {
            case Questions.Hit:
                if (deadens.canHit && deadens.enemyHit)
                {
                    trueNode.Execute(deadens);
                }
                else falseNode.Execute(deadens);
                break;
            case Questions.Idle:
                if(randomRange < 10) trueNode.Execute(deadens);
                else falseNode.Execute(deadens);
                break;
            case Questions.Move:
                if(randomRange < 30) trueNode.Execute(deadens);
                else falseNode.Execute(deadens);
                break;
            case Questions.Attack:
                if(randomRange < 70) trueNode.Execute(deadens);
                else falseNode.Execute(deadens);
                break;
            case Questions.FloorAttack:
                if (randomRange < 100) trueNode.Execute(deadens);
                else falseNode.Execute(deadens);
                break;
        }
    }

    public override void Execute(IllusionDemon i)
    {
        if (i.lastAction == actionsEnemy.NotAttack)
        {
            randomRange = Random.Range(1, 10);
        }
        switch (question)
        {
            case Questions.Hit:
                if(i.enemyHit) trueNode.Execute(i);
                else falseNode.Execute(i);
                break;
            case Questions.Move:
                if(i.lastAction == actionsEnemy.Attack) trueNode.Execute(i);
                else falseNode.Execute(i);
                break;
            case Questions.Attack:
                if(randomRange < 3) trueNode.Execute(i);
                else falseNode.Execute(i);
                break;
            case Questions.SpecialAttack:
                if(randomRange < 6) trueNode.Execute(i);
                else falseNode.Execute(i);
                break;
            case Questions.Cast:
                if(randomRange < 10) trueNode.Execute(i);
                else falseNode.Execute(i);
                break;
        }
    }

    public override void Execute(IllusionDuplication d)
    {
        switch (question)
        {
            case Questions.Move:
                if(d.lastAction == actionsEnemy.Attack) trueNode.Execute(d);
                else falseNode.Execute(d);
                break;
            case Questions.Attack:
                if(d.lastAction == actionsEnemy.NotAttack) trueNode.Execute(d);
                else falseNode.Execute(d);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if(falseNode != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, falseNode.transform.position);
        }
        if (trueNode != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, trueNode.transform.position);
        }
    }
}

public enum Questions
{
    Idle,
    Move,
    Attack,
    FloorAttack,
    Hit,
    SpecialAttack,
    Cast
    
}
