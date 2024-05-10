using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadensLifeHandler : LifeHandler
{
    public override void OnTakeDamage(int damage)
    {
        _actualLife -= damage;
        var deadensComp = GetComponent<Deadens>();
        if (_actualLife > 0)
        {
            if (deadensComp.canHit)
            {
                deadensComp.enemyHit = true;
                deadensComp.DecisionTree.Execute(deadensComp);
            }
            return;
        }

        deadensComp.canHit = false;
        EnemyManager.Instance.RemoveEnemy(GetComponent<Deadens>());
        deadensComp.mageAnim.death = true;
        ListDemonsUI.Instance.AddText(deadensComp.enemyCount, "<s><color=\"red\">Demon " + deadensComp.enemyCount + "</s></color>");
        deadensComp.GetComponent<CapsuleCollider>().enabled = false;
        deadensComp.enabled = false;
    }
}
