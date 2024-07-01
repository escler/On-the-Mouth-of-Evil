using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRDemonAnim : MonoBehaviour
{
    private Animator _animator;
    private DemonLowRange _ownerEnemy;
    public Animator Animator => _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _ownerEnemy = GetComponentInParent<DemonLowRange>();
    }

    public void SetParameter(string animName, bool state)
    {
        _animator.SetBool(animName,state);
    }

    public void StartAttack()
    {
        _ownerEnemy.StartAttack();
    }
}
