using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadensLifeHandler : LifeHandler
{
    public override void OnTakeDamage(int damage)
    {
        _actualLife -= damage;

        if (_actualLife > 0) return;

        var deadensComp = GetComponent<Deadens>();
        
        EnemyManager.Instance.RemoveEnemy(GetComponent<Deadens>());
        deadensComp.Animator.SetBool("Death",true);
        ListDemonsUI.Instance.AddText(deadensComp.enemyCount, "<s><color=\"red\">Demon " + deadensComp.enemyCount + "</s></color>");
        deadensComp.enabled = false;
        foreach (BoxCollider c in GetComponentsInChildren<BoxCollider>())
        {
            c.enabled = false;
        }
    }
}
