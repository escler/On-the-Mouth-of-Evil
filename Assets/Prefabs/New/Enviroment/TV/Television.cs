using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Television : MonoBehaviour, IInteractable
{
    public Material[] originalMaterial, onMaterial;
    private bool on;
    private Renderer mesh;
    public AudioSource _tv;

    private void Awake()
    {
        mesh = GetComponent<Renderer>();
        mesh.materials = originalMaterial;
    }

    public void ChangeMaterial()
    {
        on = !on;
        print(on);
        _tv.Play();
        mesh.materials = on ? onMaterial : originalMaterial;
    }

    public void OnInteractItem()
    {
        ChangeMaterial();
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
