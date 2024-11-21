using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubBell : MonoBehaviour, IInteractable
{
    public AudioSource bell;
    public float timeForBell;
    private float _actualTime;
    public Transform paper, finalPos;
    private bool interactUsed;

    private void Awake()
    {
        _actualTime = timeForBell / 2;
    }

    void Update()
    {
        if (interactUsed) return;
        _actualTime -= Time.deltaTime;
        if (_actualTime < 0)
        {
            bell.Play();
            _actualTime = timeForBell;
        }
    }

    IEnumerator MovePaper()
    {
        float ticks = 0;
        Vector3 originalPos = paper.position;
        while (ticks < 1)
        {
            ticks += Time.deltaTime;
            paper.position = Vector3.Lerp(originalPos, finalPos.position, ticks);
            yield return null;
        }

        paper.GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<BoxCollider>().enabled = false;
        enabled = false;
    }

    public void OnInteractItem()
    {
        if(!interactUsed) StartCoroutine(MovePaper());
        interactUsed = true;
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
        return true;
    }
}
