using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleTrigger : MonoBehaviour
{
    public Door door;
    public IllusionDemon demon;
    public SpatialGrid thisGrid;
    public GameObject nextTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            GameManager.Instance.activeSpatialGrid = thisGrid;
            demon.gameObject.SetActive(true);
            nextTrigger.SetActive(true);
            door.SetDoor(false);
            demon.OnBossDefeated += BossDefeated;
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnDestroy()
    {
        demon.OnBossDefeated -= BossDefeated;
    }

    private void BossDefeated()
    {
        door.SetDoor(true);
        GameManager.Instance.bossKilled = true;
    }
}
