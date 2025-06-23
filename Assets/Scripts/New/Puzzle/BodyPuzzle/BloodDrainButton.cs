using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDrainButton : MonoBehaviour, IInteractable
{
    public bool buttonPress;
    [SerializeField] BoxCollider collider;
    [SerializeField] BloodFX bloodFX;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void OnInteractItem()
    {
        _animator.SetTrigger("Drain");
        bloodFX.StartAnimation();
        BodyPuzzle.Instance.bloodDrained = true;
        buttonPress = true;
        collider.enabled = false;
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
        return false;
    }
}
