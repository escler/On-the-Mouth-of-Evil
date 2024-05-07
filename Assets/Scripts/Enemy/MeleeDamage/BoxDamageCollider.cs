using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDamageCollider : MonoBehaviour
{
    public int damage;

    private void OnEnable()
    {
        Destroy(gameObject, .3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            other.GetComponent<PlayerLifeHandler>().OnTakeDamage(damage);
        }
    }
}
