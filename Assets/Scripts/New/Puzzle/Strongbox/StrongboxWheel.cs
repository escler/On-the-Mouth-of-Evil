using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongboxWheel : MonoBehaviour, IInteractable, IInteractObject
{
    private bool _cantRotate;
    private int actualRotation;
    public Vector3[] rotations;
    public float speedRotation;
    private Transform model;
    private AudioSource _audiosource;

    public int number;

    private Coroutine _rotationCoroutine;

    private void Awake()
    {
        model = transform.GetChild(0);
        _audiosource = GetComponent<AudioSource>();
    }



    IEnumerator RotateWheel()
    {
        _audiosource.Play();
        _cantRotate = true;

        int prevRotation = actualRotation;
        actualRotation = (actualRotation + 1) % rotations.Length;

        Quaternion startRot = model.localRotation;
        Quaternion targetRot = Quaternion.Euler(rotations[actualRotation]);

        float elapsed = 0;
        float duration = 0.5f; // o calculado según speedRotation

        while (elapsed < duration)
        {
            model.localRotation = Quaternion.Slerp(startRot, targetRot, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        model.localRotation = targetRot;

        number = actualRotation;
        _cantRotate = false;
        _rotationCoroutine = null;
    }

    public void OnInteractItem()
    {
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public void OnInteractWithObject()
    {
        if (_cantRotate) return;

        if (_rotationCoroutine != null)
            StopCoroutine(_rotationCoroutine);

        _rotationCoroutine = StartCoroutine(RotateWheel());
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
