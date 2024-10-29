using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighterView : MonoBehaviour
{
    private Lighter owner;
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        owner = GetComponentInParent<Lighter>();
    }

    public void PlayPS()
    {
        owner.PSIdle.SetActive(!owner.PSIdle.activeInHierarchy);

    }

    public void StopPS()
    {
        owner.PSIdle.SetActive(false);
    }

}
