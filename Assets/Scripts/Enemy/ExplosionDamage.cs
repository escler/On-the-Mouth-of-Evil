using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    public int damage;
    private void OnEnable()
    {
        StartCoroutine(DisableObject());
    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(0.2f);

        transform.parent.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            other.GetComponent<PlayerLifeHandler>().TakeDamage(damage);
        }
    }
}
