using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class BossDuplicationAnim : MonoBehaviour
{
    private Animator _animator;
    public bool run;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool("Run", run);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            GetComponentInParent<LifeHandler>().OnTakeDamage(1);
        }
    }
}
