using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Television : MonoBehaviour
{
    public Material[] originalMaterial, onMaterial;
    private bool on;
    private Renderer mesh;

    private void Awake()
    {
        mesh = GetComponent<Renderer>();
        mesh.materials = originalMaterial;
    }

    public void ChangeMaterial()
    {
        on = !on;
        print(on);
        mesh.materials = on ? onMaterial : originalMaterial;
    }
}
