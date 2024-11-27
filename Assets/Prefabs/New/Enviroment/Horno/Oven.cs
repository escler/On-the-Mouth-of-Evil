using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour, IInteractable
{
    public ParticleSystem ps;
    public ParticleSystem.EmissionModule emission;
    private bool _on;
    public AudioSource _hornilla;

    private void Awake()
    {
        emission = ps.emission;
        emission.enabled = false;
    }

    public void OnInteractItem()
    {
        _on = !_on;
        if (_on)
        {
            _hornilla.Play();

        }
        else
        {
            _hornilla.Stop();
        }
        emission.enabled = _on;
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
