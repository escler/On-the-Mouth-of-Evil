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
    [SerializeField] private Transform plane, planeFinalPos;
    [SerializeField] private AudioSource pressButton, drainSound;

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
        pressButton.Play();
        drainSound.Play();
        StartCoroutine(MovePlane());
    }

    IEnumerator MovePlane()
    {
        Vector3 startPosition = plane.position;
        float time = 0;

        while (time < 1)
        {
            plane.transform.position = Vector3.Lerp(startPosition, planeFinalPos.position, time);
            time += Time.deltaTime / 4;
            yield return null;
        }
        plane.gameObject.SetActive(false);
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
