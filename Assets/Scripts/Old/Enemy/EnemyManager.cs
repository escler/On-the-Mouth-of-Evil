using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    
    private List<DemonLowRange> _enemies = new List<DemonLowRange>();
    private int enemyCount = 1;
    
    public List<DemonLowRange> Enemies => _enemies;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void AddEnemy(DemonLowRange enemy)
    {
        if (_enemies.Contains(enemy)) return;
        _enemies.Add(enemy);
        AssignNumber(enemy);
    }
    
    public void RemoveEnemy(DemonLowRange enemy)
    {
        if (!_enemies.Contains(enemy)) return;
        _enemies.Add(enemy);
    }

    public void AssignNumber(DemonLowRange enemy)
    {
        //enemy.GetComponent<Deadens>().enemyCount = enemyCount;
        enemyCount++;
    }
}
