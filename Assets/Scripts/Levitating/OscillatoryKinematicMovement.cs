using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//FINAL - Caama�o Romina - Uniform accelerated rectilinear motion hereda de Kinematic Movement
public class OscillatoryKinematicMovement : KinematicMovement
{
    //FINAL - Caama�o Romina - Encapsulamiento
    [SerializeField] protected Vector3 direction = Vector3.right;
    [SerializeField] protected float amplitude = 5f;
    [SerializeField] protected float frequency = 0.15f;
    protected float t = 0f;

    //FINAL - Caama�o Romina - Utiliza los metodos heredados y los sobreescribe
    public override Vector3 GetPositionDelta(float dt)
    {
        //calcula el mov anterior
        Vector3 prev = direction.normalized * amplitude * Mathf.Sin(2f * Mathf.PI * frequency * t);
        t += dt;//actualiza el tiempo
        //calcula el mov con el tiempo actualizado
        Vector3 curr = direction.normalized * amplitude * Mathf.Sin(2f * Mathf.PI * frequency * t);
        //devuelve la diferencia de mov
        return curr - prev;
    }
    //FINAL - Caama�o Romina - sobreescribe el metodo llamado que no hace nada originalmente
    public override void ResetMovement()
    {
        t = 0f;
    }
}
