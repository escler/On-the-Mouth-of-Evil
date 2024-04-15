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
    private Animator _animator;

    public float walkSpeed, runSpeed, sensRot;
    private float _actualSpeed;
    private bool _aiming;
    public Transform targetAim, chest;
    
    // Start is called before the first frame update
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _actualSpeed = walkSpeed;
        targetAim = GetComponentInChildren<CenterPointCamera>().transform;
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        _aiming = Input.GetMouseButton(1);
    }

    private void Update()
    {
        _animator.SetBool("Walking",_rb.velocity != Vector3.zero);
        _animator.SetBool("Aiming",_aiming);
    }

    private void Move()
    {
        if (!_aiming)
        {
            Vector3 vel = transform.forward * (_controller.GetMovementInput().x * _actualSpeed * Time.fixedDeltaTime) +
                          transform.right * (_controller.GetMovementInput().z * _actualSpeed * Time.fixedDeltaTime);
            _rb.velocity = vel;
        
            if (_rb.velocity  != Vector3.zero)
            {
                var newRot = Quaternion.Euler(0, _mainCamera.transform.eulerAngles.y, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation,newRot,sensRot * Time.fixedDeltaTime);
            }
        }
        else
        {
            _rb.velocity = Vector3.zero;
            Vector3 aimVector = targetAim.position - chest.position;
            Quaternion rotation = Quaternion.LookRotation(aimVector, Vector3.up);
            transform.rotation = rotation;
        }
    }

    private void Rotate()
    {

    }
}
