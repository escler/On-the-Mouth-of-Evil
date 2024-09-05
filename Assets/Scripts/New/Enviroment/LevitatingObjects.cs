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
    private Transform _initialPos;
    private bool interactOn;

    private void Awake()
    {
        _initialPos = transform;
    }

    private void Update()
    {
        if (!interactOn) return;
        Levitate();
    }

    public void OnStartInteract()
    {
        interactOn = true;
        _levitatingSpeed = Random.Range(levitatingSpeedMin, levitatingSpeedMax);
        _levitatingAmount = Random.Range(levitatingAmountMin, levitatingAmountMax);
        _rotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);
        Levitate();
    }

    public void OnEndInteract()
    {
    }

    private void Levitate()
    {
        _timer += Time.deltaTime * _levitatingSpeed;
        transform.localPosition = new Vector3(transform.localPosition.x, 
            transform.localPosition.y + Mathf.Cos(_timer) * _levitatingAmount,
            transform.localPosition.z);

        _timerRot += Time.deltaTime * _rotationSpeed;
        
        transform.localRotation = Quaternion.Euler(_timerRot, _timerRot,_timerRot);
    }
}
