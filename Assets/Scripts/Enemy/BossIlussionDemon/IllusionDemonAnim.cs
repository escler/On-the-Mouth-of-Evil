using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemonAnim : MonoBehaviour
{
    private Animator _animator;
    public float yAxis;

    public Animator Animator => _animator;

    public bool moving, death, hit, run, comboHit;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _animator.SetBool("Moving", moving);
        _animator.SetFloat("yAxis", yAxis);
        _animator.SetBool("Death",death);
        _animator.SetBool("Hit",hit);
        _animator.SetBool("Run", run);
        _animator.SetBool("ComboHit", comboHit);
    }
}
