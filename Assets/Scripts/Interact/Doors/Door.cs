using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private KeyType _roomType;
    [SerializeField] private Animator _animator;

    public void OnInteract()
    {
        if (KeyHandler.Instance.KeysInInventory.Contains(_roomType))
        {
            _animator.SetBool("Open", true);
            gameObject.layer = 8;
        }
    }
}
