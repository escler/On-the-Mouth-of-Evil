using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongboxWheel : MonoBehaviour, IInteractable
{
    private bool _cantRotate;
    private int actualRotation;
    public Vector3[] rotations;
    public float speedRotation;

    public int number;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) StartCoroutine(RotateWheel()); 
    }
    
    IEnumerator RotateWheel()
    {
        _cantRotate = true;
        actualRotation++;
        if (actualRotation >= rotations.Length) actualRotation = 0;
        while (Mathf.Abs(transform.localEulerAngles.z - rotations[actualRotation].z) > 1)
        {
            print(Mathf.Abs(transform.localEulerAngles.z - rotations[actualRotation].z));
            transform.Rotate(0,0,speedRotation,Space.Self);
            yield return new WaitForSeconds(0.01f);
        }

        transform.localEulerAngles = rotations[actualRotation];

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
}
