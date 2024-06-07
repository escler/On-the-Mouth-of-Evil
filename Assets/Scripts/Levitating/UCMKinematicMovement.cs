using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//FINAL - Caamano Romina - Uniform circular motion hereda de Kinematic Movement
public class UCMKinematicMovement : KinematicMovement
{
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] protected float angularVelocity = 0f;

    public override Quaternion GetRotationDelta(float dt)
    {
        return Quaternion.Euler(angularVelocity * rotationAxis * dt);
    }
}
