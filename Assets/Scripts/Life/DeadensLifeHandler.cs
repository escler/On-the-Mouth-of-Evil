using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadensLifeHandler : LifeHandler
{
    public override void OnTakeDamage(int damage)
    {
        _actualLife -= damage;
        var enemy = GetComponent<DemonLowRange>();
        if (_actualLife > 0) return;

        //EnemyManager.Instance.RemoveEnemy(GetComponent<Deadens>());
        //ListDemonsUI.Instance.AddText(deadensComp.enemyCount, "<s><color=\"red\">Demon " + deadensComp.enemyCount + "</s></color>");
        enemy.canBanish = true;
    }
}
