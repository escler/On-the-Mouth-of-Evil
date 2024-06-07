using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//FINAL - Caamaño Romina - Composicion: KinematicMovementController se compone de movimientos kinematicos varios,
//los cuales heredaron de la clase abstracta kinematic movement.

//FINAL - Caamaño Romina - requiere un rigidbody
[RequireComponent(typeof(Rigidbody))]
public class KinematicMovementController : MonoBehaviour
{
    //FINAL - Caamaño Romina - Encapsulamiento
    [SerializeField] protected float delayTime = 0f;

    protected Rigidbody rb;
    protected KinematicMovement[] kinematicMovements;
    protected bool movementEnabled = false;

    private float delay = 0f;


    protected virtual void Start()
    {
        StartMovement();
    }


    protected virtual void FixedUpdate()
    {
        if (!movementEnabled)
            return;

        float dt = Time.fixedDeltaTime;

        //FINAL - Caamano Romina - para que el delay no se haga negativo
        if (delay > 0f)
        {
            delay = Mathf.Max(delay - dt, 0f);
            return;
        }

        //FINAL - Caamano Romina - variables que van acumulando los movimientos
        Vector3 positionDelta = rb.position;
        Quaternion rotationDelta = rb.rotation;

        foreach (KinematicMovement kinematicMovement in kinematicMovements)
        {
            positionDelta += kinematicMovement.GetPositionDelta(dt);
            rotationDelta *= kinematicMovement.GetRotationDelta(dt);
        }

        //FINAL - Caamano Romina - al final aplica al rigidbody los totales
        rb.MovePosition(positionDelta);
        rb.MoveRotation(rotationDelta);
    }


    //FINAL - Caamaño Romina - Este metodo llama al reset de cada movimiento kinematico
    // entre los movimientos que hayan heredado de la clase abstracta kinematic movement
    public virtual void ResetMovement()
    {
        delay = delayTime;
        movementEnabled = false;
        foreach (KinematicMovement kinematicMovement in kinematicMovements)
        {
            kinematicMovement.ResetMovement();
        }
    }

    protected virtual void StartMovement()
    {
        delay = delayTime;
        movementEnabled = true;
    }
}
