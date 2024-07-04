using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDuplicationAnim : MonoBehaviour
{
    private Animator _animator;
    public bool run;
    private BossDuplicationMovement _mov;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _mov = GetComponentInParent<BossDuplicationMovement>();
    }

    private void Update()
    {
        _animator.SetBool("Run", _mov.run);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            GetComponentInParent<LifeHandler>().TakeDamage(1,0);
        }
    }
}
