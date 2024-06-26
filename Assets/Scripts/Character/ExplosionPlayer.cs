using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosionPlayer : MonoBehaviour
{
    [SerializeField] private CircleQuery query;
    public int dmg;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))//IA2-P2
        {
            var entities = query.Query().
                Select(x => (DemonLowRange)x).Where(x => x != null).ToList();

            var damage = DamageDone(entities);
            foreach (var entity in entities)
            {
                entity.GetComponent<DeadensLifeHandler>().OnTakeDamage(35);
            }
        }
    }

    int DamageDone(IEnumerable<DemonLowRange> entities)//IA2-P1
    {
        return entities.Aggregate(0, (acum, current) =>
        {
            var actualLife = current.GetComponent<DeadensLifeHandler>().ActualLife;
            return acum += actualLife > dmg ? dmg : dmg - actualLife;
        });
    }
}
