using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BibleView : MonoBehaviour
{
    private Animator _animator;

    public Animator animator => _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
}
