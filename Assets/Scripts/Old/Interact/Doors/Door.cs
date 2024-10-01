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
    public GameObject saltPS, triggerObstacle;

    private void Awake()
    {
        SetDoor(open);
    }

    public void OnInteractItem()
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
        saltPS.SetActive(true); //Aca prendo la sal
        doorNode.gameObject.SetActive(false);
        triggerObstacle.SetActive(true);
        DisableNodes();
        yield return new WaitForSeconds(blockDuration);
        saltBlock = false;
        saltPS.SetActive(false); //Aca apago la sal
        EnableNodes();
        triggerObstacle.SetActive(false);
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
