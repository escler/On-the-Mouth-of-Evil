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

    public void ChangeState(string state, bool value)
    {
        animator.SetBool(state, value);
    }

    public void ChangeTrigger(string trigger)
    {
        animator.SetTrigger(trigger);
    }

    public void DisableBoolCross()
    {
        animator.SetBool("CrossUsed", false);
    }

    public void ThrowVomit()
    {
        _owner.ThrowVomit();
    }

    public void PlayPs()
    {
        _owner.firePs.SetActive(true);
    }

    public void StarDissapear()
    {
        _owner.startDisappear = true;
    }
}
