using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEventGarage : MonoBehaviour, IEvent 
{
    [SerializeField] private SpawnEnemy[] _spawnEnemy;
    [SerializeField] private Door _garageDoor;
    public GameObject[] target = new GameObject [3];
    public void StartEvent()
    {
        for (int i = 0; i < _spawnEnemy.Length; i++)
        {
            _spawnEnemy[i].Spawn();
            target[i] = _spawnEnemy[i].enemy;
        }
        _garageDoor.BlockDoor();
    }

    public bool CheckEventState()
    {
        int kills = 0;
        foreach (var enemy in target)
        {
            if (enemy.GetComponent<Deadens>().enabled) break;

            kills++;
        }
        return kills == 3;
    }

    public void EndEvent()
    {
        _garageDoor.OpenDoor();
        ListDemonsUI.Instance.ClearText();
    }
    
}
