using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorgueEnemyAnim : MonoBehaviour
{
    private MorgueEnemy _owner;
    public Animator animator;

    private void Awake()
    {
        _owner = GetComponentInParent<MorgueEnemy>();
        animator = GetComponent<Animator>();
    }

    public void DisableBoolCross()
    {
        animator.SetBool("CrossUsed", false);
    }

    public void ThrowVomit()
    {
        _owner.ThrowVomit();
    }
}
