using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosionPlayer : MonoBehaviour
{
    public float radius;
    [SerializeField] private CircleQuery query;

    private void Awake()
    {
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.H))
        {
            var entities = query.Query().
                Select(x => (DemonLowRange)x).Where(x => x != null).ToList();

            print(entities.Count);
            var damage = DamageDone(entities);
            print(damage);
            foreach (var entity in entities)
            {
                entity.GetComponent<DeadensLifeHandler>().OnTakeDamage(35);
            }
            entities.Clear();

            damage = 0;
        }
        */
    }

    int DamageDone(IEnumerable<DemonLowRange> entities)
    {
        return entities.Aggregate(0, (acum, current) =>
        {
            var actualLife = current.GetComponent<DeadensLifeHandler>().ActualLife;
            return acum += actualLife > 35 ? 35 : 35 - actualLife;
        });
    }
}
