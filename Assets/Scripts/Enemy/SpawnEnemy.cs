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
        var enemySpawned = Instantiate(enemy, transform.position, transform.rotation);
        enemy = enemySpawned;
    }
    
    public void SpawnWithDelay()
    {
        StartCoroutine("DelaySpawn");
    }

    IEnumerator DelaySpawn()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        
        Spawn();
    }
    
}
