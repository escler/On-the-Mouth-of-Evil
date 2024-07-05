using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFogAttackLifeHandler : LifeHandler
{
    private FogAttackEnemy _fogAttackEnemy;

    private void Awake()
    {
        _fogAttackEnemy = GetComponent<FogAttackEnemy>();
    }

    public override void TakeDamage(int damage, int force, int hitCount)
    {
        _actualLife -= damage;
        if (_actualLife > 0) return;

        if (_fogAttackEnemy.isExplosionEnemy) transform.GetChild(0).gameObject.SetActive(true);
        else Destroy(gameObject);
    }

    private void OnDisable()
    {
        var boss = FindObjectOfType<IllusionDemon>().GetComponent<IllusionDemon>();
        boss.actualCopies--;
        boss.copyAlive = false;
    }
}
