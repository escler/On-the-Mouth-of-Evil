using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class BossDuplicationAnim : MonoBehaviour
{
    private Animator _animator;
    private BossDuplicationMovement _mov;
    public bool moving;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _mov = GetComponentInParent<BossDuplicationMovement>();
    }

    private void Update()
    {
        _animator.SetBool("Run", _mov.run);
        _animator.SetBool("Moving", moving);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            gameObject.SetActive(false);
        }
    }
}
