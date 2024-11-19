using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class LevitatingItem : MonoBehaviour
{
    public float levitatingSpeedMin, levitatingSpeedMax, levitatingAmountMin, levitatingAmountMax,
        rotationSpeedMin, rotationSpeedMax;
    public float maxHeight;
    public float velocityLevitation;
    private float _timerRot, _rotationSpeed, _levitatingSpeed, _levitatingAmount;
    private Vector3 initialPos, _endPos, _actualPos;
    public bool grabbed;
    private float ticks;
    private Vector3 originalPos, originalForward;


    private void Awake()
    {
        initialPos = transform.position;
        _levitatingSpeed = Random.Range(levitatingSpeedMin, levitatingSpeedMax);
        _levitatingAmount = Random.Range(levitatingAmountMin, levitatingAmountMax);
        _rotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);
        ticks = 0;
    }

    private void Update()
    {
        Levitate();
        GoToHand();
    }

    private void Levitate()
    {
        if (grabbed) return;

        transform.position = initialPos + new Vector3(0, 
            Mathf.Sin(Time.time * velocityLevitation) * maxHeight, 0);

        _timerRot += Time.deltaTime * _rotationSpeed;
        transform.localRotation = Quaternion.Euler(0, _timerRot,0);
    }

    private void GoToHand()
    {
        if (!grabbed)
        {
            originalPos = transform.position;
            originalForward = transform.forward;
            
            return;
        }

        Vector3 dir = PlayerHandler.Instance.cameraPos.position - transform.position;

        ticks += Time.deltaTime;
        transform.position = Vector3.Lerp(originalPos, PlayerHandler.Instance.handPivot.position, ticks);
        transform.forward = Vector3.Lerp(originalForward, dir, ticks);
    }

}
