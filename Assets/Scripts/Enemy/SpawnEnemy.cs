using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemy;
    private bool _isSummonByBoss;
    public void Spawn()
    {
        var enemySpawned = Instantiate(enemy, transform.position, transform.rotation);
        enemy = enemySpawned;
        enemy.GetComponent<Deadens>().summonedByBoss = _isSummonByBoss;
    }
    
    public void SpawnWithDelay(bool summonByBoss)
    {
        StartCoroutine("DelaySpawn");
        _isSummonByBoss = summonByBoss;
    }

    IEnumerator DelaySpawn()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        
        Spawn();
    }
    
}
