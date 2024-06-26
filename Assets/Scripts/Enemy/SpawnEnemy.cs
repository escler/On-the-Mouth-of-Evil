using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemy;
    private bool _isSummonByBoss;
    public Transform pivotToSpawn;
    public void Spawn()
    {
        var enemySpawned = Instantiate(enemy, pivotToSpawn.transform.position, pivotToSpawn.transform.rotation);
        enemy = enemySpawned;
    }
    
    public void SpawnWithDelay()
    {
        StartCoroutine("DelaySpawn");
    }

    IEnumerator DelaySpawn()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(Random.Range(1,4));
        
        Spawn();
    }
    
}
