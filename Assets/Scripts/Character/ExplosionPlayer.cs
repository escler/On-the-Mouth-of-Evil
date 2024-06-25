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
        if (Input.GetKeyDown(KeyCode.H))
        {
            var entities = query.Query().
                Select(x => (DemonLowRange)x).Where(x => x != null).ToList();
            foreach (var entity in entities)
            {
                entity.GetComponent<DeadensLifeHandler>().OnTakeDamage(20);
            }
        }
    }
}
