using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogPlayer : MonoBehaviour
{
    private Animator _animator;

    public bool start;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Update()
    {
        _animator.SetBool("Start", start);
    }
}
