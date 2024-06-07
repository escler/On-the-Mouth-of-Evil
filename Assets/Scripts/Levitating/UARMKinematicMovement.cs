using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//FINAL - Caama�o Romina - Uniform accelerated rectilinear motion hereda de Kinematic Movement
public class UARMKinematicMovement : KinematicMovement
{
    //FINAL - Caama�o Romina - Encapsulamiento
    [SerializeField] protected Vector3 acceleration = Vector3.zero;
    [SerializeField] protected Vector3 initialVelocity = Vector3.zero;

    protected Vector3 velocity;

    //FINAL - Caama�o Romina - Utiliza la funcion de reset en que no hace nada, solo para que tenga un awake
    protected virtual void Awake()
    {
        ResetMovement();
    }
    //FINAL - Caama�o Romina - Utiliza los metodos heredados y los sobreescribe
    public override Vector3 GetPositionDelta(float dt)
    {
        velocity += acceleration * dt;
        return velocity * dt;
    }
    //FINAL - Caama�o Romina - sobreescribe el metodo llamado que no hace nada originalmente
    public override void ResetMovement()
    {
        velocity = initialVelocity;
    }
}
