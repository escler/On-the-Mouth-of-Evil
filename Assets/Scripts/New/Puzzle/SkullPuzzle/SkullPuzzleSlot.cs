using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullPuzzleSlot : MonoBehaviour, IInteractable
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

    private void Awake()
    {
        slotTransform = transform.GetChild(0);
        offset = slotTransform.localEulerAngles.y;
    }

    public void PlaceSkull(Skull skull)
    {
        if (currentSkull != null) return;
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
    }

    IEnumerator WaitCor()
    {
        yield return new WaitForEndOfFrame();
        _cantRotate = false;
    }

    public void GrabSkull()
    {
        if (currentSkull == null) return;
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
            print("Entre aca");
            //transform.eulerAngles = Vector3.SmoothDamp(transform.eulerAngles, rotations[actualRotation], ref reference, .5f);
            yield return new WaitForSeconds(0.01f);
        }

        slotTransform.localEulerAngles = rotations[actualRotation] + new Vector3(0,offset,0);
        
        SkullPuzzle.Instance.CheckPuzzleState();

        _cantRotate = false;
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
