using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopDrawer : MonoBehaviour, IInteractable
{
    private bool _cantInteract;
    private Vector3 _startPosition;
    public float offset;
    private bool _open;
    public AudioSource openDrawer;
    public AudioSource closeDrawer;

    private void Awake()
    {
        _startPosition = transform.localPosition;
    }

    public void OnInteractItem()
    {
        Interact();
    }

    private void Interact()
    {
        if (_cantInteract) return;
        _cantInteract = true;
        _open = !_open;
        StartCoroutine(_open ? OpenDrawer() : CloseDrawer());
    }

    IEnumerator CloseDrawer()
    {
        float time = 0;
        Vector3 position = transform.localPosition;

        closeDrawer.Play();
        while (time < 1)
        {
            time += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(position, _startPosition, time);
            yield return null;
        }

        _cantInteract = false;
    }

    IEnumerator OpenDrawer()
    {
        float time = 0;
        Vector3 position = transform.localPosition;

        openDrawer.Play();
        while (time < 1)
        {
            time += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(position, _startPosition - transform.forward * offset, time);
            yield return null;
        }

        _cantInteract = false;
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
        return !_cantInteract;
    }
}
