using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevitatingObjects : MonoBehaviour, IInteractableEnemy
{
    public float levitatingSpeedMin, levitatingSpeedMax, levitatingAmountMin, levitatingAmountMax,
        rotationSpeedMin, rotationSpeedMax;
    private float _timer, _timerRot, _rotationSpeed, _levitatingSpeed, _levitatingAmount;
    private Vector3 _initialPos, _endPos, _actualPos;
    private Vector3 reference = Vector3.zero;
    private bool interactOn;
    private Rigidbody _rb;

    private void Awake()
    {
        _initialPos = transform.localPosition;
        _actualPos = _initialPos;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!interactOn) return;
        Levitate();
    }

    public void OnStartInteract()
    {
        interactOn = true;
        _rb.isKinematic = true;
        _levitatingSpeed = Random.Range(levitatingSpeedMin, levitatingSpeedMax);
        _levitatingAmount = Random.Range(levitatingAmountMin, levitatingAmountMax);
        _rotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);
        _endPos = _initialPos + Vector3.up * _levitatingAmount;
    }

    public void OnEndInteract()
    {
        _rb.isKinematic = false;
        interactOn = false;
    }

    private void Levitate()
    {
        _timer += Time.deltaTime * _levitatingSpeed;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, transform.localPosition + Vector3.up * _levitatingAmount,
            ref reference,_levitatingSpeed);

        if (Vector3.Distance(transform.localPosition, _actualPos) < 0.1f)
        {
            var initialPos = _actualPos == _initialPos ? _endPos : _initialPos;
            _actualPos = initialPos;
        }

        _timerRot += Time.deltaTime * _rotationSpeed;
        transform.localRotation = Quaternion.Euler(_timerRot, _timerRot,_timerRot);
    }

}
