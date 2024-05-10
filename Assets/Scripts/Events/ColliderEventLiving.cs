using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEventLiving : MonoBehaviour, IEvent
{
    [SerializeField] private SpawnEnemy _spawnEnemy;
    [SerializeField] private Door _livingDoor;
    public GameObject target;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            _spawnEnemy.Spawn();
            EventsManager.Instance.SetCurrentEvent(this);
        }
    }

    public void StartEvent()
    {
        _livingDoor.BlockDoor();
        GetComponent<Collider>().enabled = false;
        target = _spawnEnemy.enemy;
    }

    public bool CheckEventState()
    {
        return !target.GetComponent<Deadens>().enabled;
    }


    public void EndEvent()
    {
        _livingDoor.OpenDoor();
        ListDemonsUI.Instance.ClearText();
    }
}
