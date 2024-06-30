using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEnemies : MonoBehaviour
{
    public GameObject zone;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6) zone.SetActive(true);
    }
}
