using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZoneManager : MonoBehaviour
{
    private BoxCollider _collider;
    [SerializeField] private GameObject spawnPrefab;
    public int count;

    private void OnEnable()
    {
        _collider = GetComponent<BoxCollider>();
        for (int i = 0; i < count; i++)
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        var randomPos = RandomPointInBounds(_collider.bounds);
        var spawn = Instantiate(spawnPrefab, new Vector3(randomPos.x, transform.position.y, randomPos.z), transform.rotation);
        spawn.GetComponent<SpawnEnemy>().SpawnWithDelay();
    }
    
    public static Vector3 RandomPointInBounds(Bounds bounds) {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
