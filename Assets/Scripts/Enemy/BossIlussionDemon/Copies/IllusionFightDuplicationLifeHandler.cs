using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionFightDuplicationLifeHandler : LifeHandler
{
    public override void OnTakeDamage(int damage)
    {
        _actualLife -= damage;
        if (_actualLife > 0) return;

        FindObjectOfType<IllusionDemon>().fightingCopies--;
        Destroy(gameObject);

    }
}
