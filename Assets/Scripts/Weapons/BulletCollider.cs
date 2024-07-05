using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollider : MonoBehaviour
{
    public int speed, damage;

    private void OnEnable()
    {
        transform.LookAt(Player.Instance.transform);
    }

    private void Update()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            other.GetComponent<PlayerLifeHandler>().TakeDamage(damage, 0, 0);
        }
        
        Destroy(gameObject);
    }
}
