using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class LevitatingDoll : MonoBehaviour
{
    public float levitatingSpeedMin, levitatingSpeedMax, levitatingAmountMin, levitatingAmountMax,
        rotationSpeedMin, rotationSpeedMax;
    public float maxHeight;
    public float velocityLevitation;
    private float _timer, _timerRot, _rotationSpeed, _levitatingSpeed, _levitatingAmount;
    private Vector3 initialPos, _endPos, _actualPos;
    private Vector3 reference = Vector3.zero;
    private bool interactOn;


    private void Awake()
    {
        initialPos = transform.position;
        _levitatingSpeed = Random.Range(levitatingSpeedMin, levitatingSpeedMax);
        _levitatingAmount = Random.Range(levitatingAmountMin, levitatingAmountMax);
        _rotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);
    }

    private void Update()
    {
        Levitate();
    }

    private void Levitate()
    {
        _timer += Time.deltaTime * _levitatingSpeed;

        transform.position = initialPos + new Vector3(0, 
            Mathf.Sin(Time.time * velocityLevitation) * maxHeight, 0);

        _timerRot += Time.deltaTime * _rotationSpeed;
        transform.localRotation = Quaternion.Euler(0, _timerRot,0);
    }

}
