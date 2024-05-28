using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemonLifeHandler : LifeHandler
{
    public override void OnTakeDamage(int damage)
    {
        _actualLife -= damage;
        var illusionDemon = GetComponent<IllusionDemon>();
        if (_actualLife > 0)
        {
            if (illusionDemon.canHit)
            {
                illusionDemon.enemyHit = true;
                //illusionDemon.DecisionTree.Execute(illusionDemon);
            }
            return;
        }

        illusionDemon.canHit = false;
        illusionDemon.Anim.death = true;
        illusionDemon.GetComponentInChildren<CapsuleCollider>().enabled = false;
        illusionDemon.enabled = false;
    }
}
