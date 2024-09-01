using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed, runSpeed;
    private float _actualSpeed, _inputX, _inputY;
    private Rigidbody _rb;
    private bool _run;

    public bool Run => _run;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
        _inputY = Input.GetAxisRaw("Vertical");
        _run = Input.GetButton("Run");
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        _actualSpeed = _run ? runSpeed : walkSpeed;

        var inputVector = new Vector2(_inputX, _inputY);
        Vector3 velocity = transform.forward * inputVector.y + transform.right * inputVector.x;
        velocity.Normalize();

        _rb.velocity = velocity * (_actualSpeed * Time.fixedDeltaTime);
    }
}
