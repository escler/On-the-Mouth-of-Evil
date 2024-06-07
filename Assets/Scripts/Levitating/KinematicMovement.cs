using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//FINAL - Caamaño Romina - requiere el Kinematic Movement controller
[RequireComponent(typeof(KinematicMovementController))]

//FINAL - Caamaño Romina - clase abstracta para los que los distintos movimientos kinematicos sean movimientos kinemativos
public abstract class KinematicMovement : MonoBehaviour
{
    //FINAL - Caamaño Romina - metodos que devuelven los incrementos en el movimiento
    public virtual Vector3 GetPositionDelta(float dt)
    {
        return Vector3.zero;
    }

    public virtual Quaternion GetRotationDelta(float dt)
    {
        return Quaternion.identity;
    }

    //FINAL - Caamaño Romina - metodo que no realiza nada y se llama en el awake de UARMkinematic Movement, para que sus hijos lo hereden
    public virtual void ResetMovement()
    {
    }
}
