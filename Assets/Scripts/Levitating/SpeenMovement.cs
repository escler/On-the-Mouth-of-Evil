using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeenMovement : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float angularVelocity = 1f;

   
    void FixedUpdate()
    {
        transform.Rotate(rotationAxis * angularVelocity * Time.deltaTime);
    }

    

}
