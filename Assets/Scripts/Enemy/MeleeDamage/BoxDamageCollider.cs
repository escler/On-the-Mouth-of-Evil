using System;
using System.Collections;
using UnityEngine;

public class BoxDamageCollider : MonoBehaviour
{
    public int damage;

    private void OnEnable()
    {
        StartCoroutine(DisableObject());
    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            other.GetComponent<PlayerLifeHandler>().TakeDamage(damage,0);
        }
    }
}
