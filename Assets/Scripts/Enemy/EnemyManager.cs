using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    
    private List<EnemySteeringAgent> _enemies = new List<EnemySteeringAgent>();
    private int enemyCount = 1;
    
    public List<EnemySteeringAgent> Enemies => _enemies;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void AddEnemy(EnemySteeringAgent enemy)
    {
        if (_enemies.Contains(enemy)) return;
        _enemies.Add(enemy);
        AssignNumber(enemy);
    }
    
    public void RemoveEnemy(EnemySteeringAgent enemy)
    {
        if (!_enemies.Contains(enemy)) return;
        _enemies.Add(enemy);
    }

    public void AssignNumber(EnemySteeringAgent enemy)
    {
        //enemy.GetComponent<Deadens>().enemyCount = enemyCount;
        enemyCount++;
    }
}
