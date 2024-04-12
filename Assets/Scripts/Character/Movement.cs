using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Controller _controller;
    [SerializeField] private Transform _mainCamera;
    private Rigidbody _rb;

    public float walkSpeed, runSpeed, sensRot;
    private float _actualSpeed;
    
    // Start is called before the first frame update
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _actualSpeed = walkSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 vel = transform.forward * (_controller.GetMovementInput().x * _actualSpeed * Time.fixedDeltaTime) +
                      transform.right * (_controller.GetMovementInput().z * _actualSpeed * Time.fixedDeltaTime);
        _rb.velocity = vel;
    }
}
