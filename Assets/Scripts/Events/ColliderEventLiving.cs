using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEventLiving : MonoBehaviour
{
    [SerializeField] private SpawnEnemy _spawnEnemy;
    [SerializeField] private Door _livingDoor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            _spawnEnemy.Spawn();
            _livingDoor.BlockDoor();
            GetComponent<Collider>().enabled = false;
        }
    }
}
