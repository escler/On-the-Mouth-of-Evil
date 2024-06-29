using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosionPlayer : MonoBehaviour
{
    [SerializeField] private CircleQuery query;
    public List<Enemy> entities = new List<Enemy>();
    public int dmg;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))//IA2-P2
        {
            entities = query.Query().
                Select(x => (Enemy)x).Where(x => x != null).ToList();

            var damage = DamageDone(entities);
            foreach (var entity in entities)
            {
                entity.Life.OnTakeDamage(dmg);
            }
            
            print(damage);
        }
    }

    int DamageDone(IEnumerable<Enemy> entities)//IA2-P1
    {
        return entities.Aggregate(0, (acum, current) =>
        {
            var actualLife = current.GetComponent<LifeHandler>().ActualLife;
            acum += actualLife > dmg ? dmg : dmg - actualLife;
            return acum;
        });
    }
}
