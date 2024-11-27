using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
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
        if (on)
        {
            _tv.Play();

        }
        else
        {
           _tv.Stop();
        }
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
