using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knob : MonoBehaviour, IInteractable, IInteractObject
{
    private bool _on;
    public int number;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnInteractItem()
    {
    }

    public void ResetKnob()
    {
        _on = false;
        _animator.SetBool("Open", _on);
    }
    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public void OnInteractWithObject()
    {
        Oven.Instance.ChangeFireState(this);
        if (_on)
        {
            Oven.Instance.RemoveKnob(this);
        }
        else
        {   
            Oven.Instance.AddKnob(this);
        }

        _on = !_on;
        _animator.SetBool("Open", _on);
    }

    public string ShowText()
    {
        return "";
    }

    public bool CanShowText()
    {
        return false;
    }

    public void OnInteractWithThisObject()
    {
        OnInteractWithObject();
    }
}
