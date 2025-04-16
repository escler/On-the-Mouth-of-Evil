using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenDoor : MonoBehaviour, IInteractable
{
    Animator _animator;
    private bool _open;
    public bool Open
    {
        get
        {
            return _open;
        }
        set
        {
            _open = value;
        }
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnInteractItem()
    {
        _open = !_open;
        _animator.SetBool("Open", _open);
    }

    public void ResetDoor()
    {
        _open = false;
        _animator.SetBool("Open", _open);
    }

    public void OpendDoor()
    {
        _open = true;
        _animator.SetBool("Open", _open);
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public void OnInteractWithObject()
    {
    }

    public string ShowText()
    {
        return "";
    }

    public bool CanShowText()
    {
        return true;
    }
}
