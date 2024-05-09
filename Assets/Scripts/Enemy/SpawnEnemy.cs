using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemy;

    public void Spawn()
    {
        var enemySpawned = Instantiate(this.enemy, transform.position, transform.rotation);
        enemy = enemySpawned;
    }
}
