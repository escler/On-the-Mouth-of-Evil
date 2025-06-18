using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullPuzzleSlot : MonoBehaviour, IInteractable, IInteractObject
{
    public Skull currentSkull;
    public Transform slotTransform;
    public int correctRotation;
    public int actualRotation;
    public Vector3[] rotations;
    private bool _cantRotate;
    private Vector3 reference = Vector3.zero;
    public float speedRotation;
    public Mark mark;
    private float offset;
    private bool _cantGrab;
    public AudioSource _girar;
    public int numberSlot;
    public GameObject[] correctPs;

    private void Awake()
    {
        slotTransform = transform.GetChild(0);
        offset = slotTransform.localEulerAngles.y;
    }

    public void PlaceSkull(Skull skull)
    {
        if (currentSkull != null) return;
        StartCoroutine(WaitToGrab());
        _cantRotate = true;
        StartCoroutine(WaitCor());
        currentSkull = skull;
        Inventory.Instance.DropItem(Inventory.Instance.selectedItem, Inventory.Instance.countSelected);
        currentSkull.GetComponent<Rigidbody>().isKinematic = true;
        currentSkull.GetComponent<BoxCollider>().enabled = false;
        currentSkull.transform.position = transform.position;
        currentSkull.transform.rotation = slotTransform.rotation;
        currentSkull.transform.SetParent(slotTransform);
        SkullPuzzle.Instance.CheckPuzzleState();
        CheckCorrectState();
    }

    public void InteractWithSkull(Skull skull)
    {
        if(currentSkull == null) PlaceSkull(skull);
        else GrabSkull();
    }
    
    IEnumerator WaitCor()
    {
        yield return new WaitForEndOfFrame();
        _cantRotate = false;
    }

    public void GrabSkull()
    {
        if (currentSkull == null || _cantGrab) return;
        currentSkull.GetComponent<Rigidbody>().isKinematic = false;
        currentSkull.GetComponent<BoxCollider>().enabled = true;
        currentSkull.OnGrabItem();
        currentSkull = null;
    }

    public void OnInteractItem()
    {
        GrabSkull();
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
        
    }
    
    public void OnInteractWithObject()
    {
        if (_cantRotate) return;
        StartCoroutine(RotateSocket());
    }

    IEnumerator RotateSocket()
    {
        _cantRotate = true;
        actualRotation++;
        if (actualRotation >= rotations.Length) actualRotation = 0;
        while (Mathf.Abs(slotTransform.localEulerAngles.y - rotations[actualRotation].y + offset) > 1)
        {
            slotTransform.Rotate(0,speedRotation,0,Space.Self);
            yield return new WaitForSeconds(0.01f);
        }

        slotTransform.localEulerAngles = rotations[actualRotation] + new Vector3(0,offset,0);
        
        SkullPuzzle.Instance.CheckPuzzleState();

        _cantRotate = false;
        CheckCorrectState();
    }
    

    public string ShowText()
    {
        return "";
    }

    public void DisableSlot()
    {
        GetComponent<BoxCollider>().enabled = false;
        enabled = false;
    }

    public bool CanShowText()
    {
        return false;
    }

    IEnumerator WaitToGrab()
    {
        _cantGrab = true;
        yield return new WaitForSeconds(0.1f);
        _cantGrab = false;
    }

    public void OnInteractWithThisObject()
    {
        OnInteractWithObject();
    }

    private void CheckCorrectState()
    {
        if (currentSkull == null) return;
        if (numberSlot != currentSkull.numberSlot) return;
        if (correctRotation != actualRotation) return;

        foreach (var c in correctPs)
        {
            c.SetActive(true);
        }
    }
}
