using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDamage : MonoBehaviour
{
    public int damagePerSecond;
    private bool _inContact;
    private float actualTime;

    private void Awake()
    {
        actualTime = 1;
        Destroy(gameObject, 5);
    }

    private void Update()
    {
        if (_inContact)
        {
            actualTime -= Time.deltaTime;
        }

        if (actualTime <= 0)
        {
            Player.Instance.GetComponent<PlayerLifeHandler>().TakeDamage(damagePerSecond,0);
            actualTime = 1f;
        }
        
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            _inContact = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            _inContact = false;
        }
    }
    
    
}
