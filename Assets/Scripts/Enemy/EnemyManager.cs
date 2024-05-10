using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    
    private List<SteeringAgent> _enemies = new List<SteeringAgent>();
    private int enemyCount = 1;
    
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
        AssignNumber(enemy);
    }
    
    public void RemoveEnemy(SteeringAgent enemy)
    {
        if (!_enemies.Contains(enemy)) return;
        _enemies.Add(enemy);
    }

    public void AssignNumber(SteeringAgent enemy)
    {
        enemy.GetComponent<Deadens>().enemyCount = enemyCount;
        enemyCount++;
    }
}
