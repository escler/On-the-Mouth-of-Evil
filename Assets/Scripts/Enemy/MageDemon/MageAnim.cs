using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAnim : MonoBehaviour
{
    private Animator _animator;
    public bool moving, hit, death, floorAttack, normalAttack;
    public float _xAxis, _yAxis;
    private Deadens _deadens;

    public Animator Animator => _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _deadens = GetComponentInParent<Deadens>();
    }

    private void Update()
    {
        _animator.SetFloat("xAxis", _xAxis);
        _animator.SetFloat("yAxis", _yAxis);
        _animator.SetBool("Moving", moving);
        _animator.SetBool("Attack", normalAttack);
        _animator.SetBool("FloorAttack", floorAttack);
        _animator.SetBool("Death", death);
        _animator.SetBool("Hit", hit);
    }

    public void AdjustPosition()
    {
        var modelPos = _deadens._model.transform.position;
        _deadens.transform.position = new Vector3(modelPos.x, _deadens.transform.position.y, modelPos.z);
        _deadens._model.transform.localPosition = Vector3.zero;
    }

    public void Attack()
    {
        _deadens.Attack();
    }

    public void FloorAttack()
    {
        _deadens.FloorAttack();
    }
}
