using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardPuzzle : MonoBehaviour
{
    public float rotationSpeed = 90f; // Velocidad de rotación

    private void Update()
    {
 
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateSelectedObject();
        }
    }
    private void RotateSelectedObject()
    {
        // Obtén el objeto seleccionado (puedes implementar tu propia lógica aquí)
        GameObject selectedObject = GetSelectedObject();

        if (selectedObject != null)
        {
            // Rota el objeto en el eje Y (horizontal)
            selectedObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    private GameObject GetSelectedObject()
    {
        // Implementa tu lógica para seleccionar el objeto (raycasting, colisiones, etc.)
        // Devuelve el objeto seleccionado o null si no hay ninguno seleccionado
        // Por ejemplo:
        // Raycast desde la cámara y devuelve el objeto golpeado
        // ...

        return null; // Cambia esto según tu implementación
    }
}

