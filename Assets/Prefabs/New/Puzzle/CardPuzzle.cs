using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardPuzzle : MonoBehaviour
{
    public float rotationSpeed = 90f; // Velocidad de rotaci�n

    private void Update()
    {
 
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateSelectedObject();
        }
    }
    private void RotateSelectedObject()
    {
        // Obt�n el objeto seleccionado (puedes implementar tu propia l�gica aqu�)
        GameObject selectedObject = GetSelectedObject();

        if (selectedObject != null)
        {
            // Rota el objeto en el eje Y (horizontal)
            selectedObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    private GameObject GetSelectedObject()
    {
        // Implementa tu l�gica para seleccionar el objeto (raycasting, colisiones, etc.)
        // Devuelve el objeto seleccionado o null si no hay ninguno seleccionado
        // Por ejemplo:
        // Raycast desde la c�mara y devuelve el objeto golpeado
        // ...

        return null; // Cambia esto seg�n tu implementaci�n
    }
}

