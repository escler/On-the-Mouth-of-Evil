using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private KeyType _roomType;
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        if(_roomType == KeyType.Key_default) OpenDoor();
    }

    public void OnInteract()
    {
        if (KeyHandler.Instance.KeysInInventory.Contains(_roomType))
        {
            OpenDoor();
            gameObject.layer = 8;
        }
    }

    public void BlockDoor()
    {
        _animator.SetBool("Open", false);
    }

    public void OpenDoor()
    {
        _animator.SetBool("Open", true);
    }
}
