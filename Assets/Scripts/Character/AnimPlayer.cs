using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimPlayer : MonoBehaviour
{
    private Animator _animator;
    private RigBuilder _rig;
    [SerializeField] private Controller _controller;
    private Rigidbody _rb;
    private bool _aiming, _shotgun, _running;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _rig = GetComponentInChildren<RigBuilder>();

        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _aiming = Input.GetMouseButton(1);
        _running = Input.GetButton("Run") && !_aiming;
        _animator.SetFloat("AxisX",_controller.GetMovementInput().x);
        _animator.SetFloat("AxisY",_controller.GetMovementInput().z);
        _animator.SetBool("Walking",_rb.velocity != Vector3.zero);
        _animator.SetBool("Aiming",_aiming);
        _animator.SetBool("Running", _running);

        EnableRig();
    }

    private void EnableRig()
    {
        if (_aiming)
        {
            if (!_shotgun)
            {
                _rig.layers[0].active = true;
                _rig.layers[1].active = false;
                _rig.layers[2].active = false;
            }
            else
            {
                _rig.layers[0].active = false;
                _rig.layers[1].active = false;
                _rig.layers[2].active = true;
            }

        }
        else
        {
            _rig.layers[0].active = false;
            _rig.layers[1].active = true;
            _rig.layers[2].active = false;
        }
    }

    public void ChangeLayerHeight(int value)
    {
        switch (value)
        {
            case 0:
                _animator.SetLayerWeight(1,1);
                _animator.SetLayerWeight(2,0);
                _shotgun = false;
                break;
            case 1:
                _animator.SetLayerWeight(1,0);
                _animator.SetLayerWeight(2,1);
                _shotgun = true;
                break;
        }

    }
}
