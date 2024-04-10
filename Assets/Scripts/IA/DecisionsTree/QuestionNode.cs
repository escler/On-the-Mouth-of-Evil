using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionNode : DecisionNode
{
    public DecisionNode falseNode, trueNode;

    public Questions question;

    public override void Execute(Deadens deadens)
    {
        switch (question)
        {
            case Questions.CanAttack:
                if(deadens.CanAttack) trueNode.Execute(deadens);
                else falseNode.Execute(deadens);
                break;
            case Questions.PlayerInFOV:
                if(deadens.PlayerInFov) trueNode.Execute(deadens);
                else falseNode.Execute(deadens);
                break;
            case Questions.WallBetweenPlayerAndMe:
                if(deadens.LineOfSight()) trueNode.Execute(deadens);
                else falseNode.Execute(deadens);
                break;
            case Questions.PlayerInAttackRange:
                if (deadens.InRangeForAttack) trueNode.Execute(deadens);
                else falseNode.Execute(deadens);
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
    CanAttack,
    PlayerInFOV,
    WallBetweenPlayerAndMe,
    PlayerInAttackRange
    
}
