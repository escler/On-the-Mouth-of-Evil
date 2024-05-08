using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;

    public void Spawn()
    {
        Instantiate(_enemy, transform.position, transform.rotation);
    }
}
