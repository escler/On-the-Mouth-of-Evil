using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleTrigger : MonoBehaviour
{
    public Door door;
    public IllusionDemon demon;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            door.BlockDoor();
            demon.gameObject.SetActive(true);
        }
    }
}
