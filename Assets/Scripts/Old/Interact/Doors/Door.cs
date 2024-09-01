using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private KeyType _roomType;
    [SerializeField] private Animator _animator;
    [SerializeField] private List<Node> doorNodes;
    public bool open, test;
    public string interactTextOpen, interactTextClose;

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
        yield return new WaitForSeconds(1f);
        DisableNodes();
        yield return new WaitForSeconds(20f);
        EnableNodes();
    }

    public void DisableNodes()
    {
        foreach (var node in doorNodes)
        {
            node.gameObject.SetActive(false);
        }
    }

    public void EnableNodes()
    {
        foreach (var node in doorNodes)
        {
            node.gameObject.SetActive(true);

        }
    }

    public void SetDoor(bool state)
    {
        _animator.SetBool("Open", state);
    }
}
