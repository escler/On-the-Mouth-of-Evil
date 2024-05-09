using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    
    private List<SteeringAgent> _enemies = new List<SteeringAgent>();
    
    public List<SteeringAgent> Enemies => _enemies;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void AddEnemy(SteeringAgent enemy)
    {
        if (_enemies.Contains(enemy)) return;
        _enemies.Add(enemy);
    }
    
    public void RemoveEnemy(SteeringAgent enemy)
    {
        if (!_enemies.Contains(enemy)) return;
        _enemies.Add(enemy);
    }
}
