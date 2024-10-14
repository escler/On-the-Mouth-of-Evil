using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissableCollider : MonoBehaviour
{
    [SerializeField] private Collider _collider;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _collider.enabled = false;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _collider.enabled = true;
        }
    }
}


