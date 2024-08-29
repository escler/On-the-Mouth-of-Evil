using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private KeyType _roomType;
    [SerializeField] private Animator _animator;
    public bool open;

    private void Awake()
    {
        SetDoor(open);
    }

    public void OnInteract()
    {
        
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
        
    }


    public void SetDoor(bool state)
    {
        _animator.SetBool("Open", state);
    }
}
