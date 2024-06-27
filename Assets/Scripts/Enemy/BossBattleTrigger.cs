using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleTrigger : MonoBehaviour
{
    public Door door;
    public IllusionDemon demon;
    public SpatialGrid thisGrid;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            GameManager.Instance.activeSpatialGrid = thisGrid;
            demon.gameObject.SetActive(true);
            door.SetDoor(false);
        }
    }
}
