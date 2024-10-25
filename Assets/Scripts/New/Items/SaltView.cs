using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltView : MonoBehaviour
{
    public Animator animator;
    private Salt _owner;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        _owner = GetComponentInParent<Salt>();
    }
    
    public void PlacingBool()
    {
        animator.SetBool("PuzzleSalt", true);
    }

    public void DisableBool()
    {
        animator.SetBool("PuzzleSalt", false);
    }

    public void ParticlePlay()
    {
        _owner.ps.Play();
    }

    public void ParticlePause()
    {
        _owner.ps.Stop();
    }

    public void ParticleStop()
    {
        _owner.ps.Stop();
    }
}
