using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDamage : MonoBehaviour
{
    public int damagePerSecond;
    private bool _inContact;
    private float actualTime;
    [SerializeField] GameObject _player;

    private void Awake()
    {
        actualTime = 2;
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
            _player.GetComponent<LifeHandler>()?.OnTakeDamage(damagePerSecond);
            actualTime = 2;
        }
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            _player = other.gameObject;
            _inContact = true;
            actualTime = 2;
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
