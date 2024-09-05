using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour, IInteractableEnemy
{
    private Rigidbody _rb;
    public float force;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void OnStartInteract()
    {
        Throw();
    }

    public void OnEndInteract()
    {
    }

    private void Throw()
    {
        _rb.AddForce(transform.forward * force, ForceMode.Impulse);
    }
}
