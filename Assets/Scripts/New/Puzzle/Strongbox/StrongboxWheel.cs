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


    private void Awake()
    {
        model = transform.GetChild(0);
        _audiosource = GetComponent<AudioSource>();
    }

    IEnumerator RotateWheel()
    {
        _audiosource.Play();
        _cantRotate = true;
        actualRotation++;
        if (actualRotation >= rotations.Length) actualRotation = 0;
        while (Mathf.Abs(model.localEulerAngles.z - rotations[actualRotation].z) > 1)
        {
            print(Mathf.Abs(model.localEulerAngles.z - rotations[actualRotation].z));
            model.Rotate(0,0,speedRotation,Space.Self);
            yield return new WaitForSeconds(0.01f);
        }

        model.localEulerAngles = rotations[actualRotation];

        number = actualRotation;
        _cantRotate = false;
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
        StartCoroutine(RotateWheel());
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
