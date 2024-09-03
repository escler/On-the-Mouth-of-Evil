using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private KeyType _roomType;
    [SerializeField] private Animator _animator;
    [SerializeField] private Node doorNode;
    public bool open, test;
    public string interactTextOpen, interactTextClose;
    public bool saltBlock;
    public float blockDuration;
    public Room[] rooms;

    private void Awake()
    {
        SetDoor(open);
    }

    public void OnInteract()
    {
        open = !open;
        SetDoor(open);
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
        
    }

    public string ShowText()
    {
        return open ? interactTextOpen : interactTextClose;
    }

    public void BlockDoor()
    {
        StartCoroutine(BlockDoorCor());
    }

    private IEnumerator BlockDoorCor()
    {
        doorNode.gameObject.SetActive(false);
        DisableNodes();
        yield return new WaitForSeconds(blockDuration);
        saltBlock = false;
        EnableNodes();
    }

    public void DisableNodes()
    {
        saltBlock = true;
        foreach (var room in rooms)
        {
            room.CheckDoors();
        }
    }

    public void EnableNodes()
    {
        foreach (var room in rooms)
        {
            room.EnableNodes();
        }
    }

    public void SetDoor(bool state)
    {
        _animator.SetBool("Open", state);
    }
}
