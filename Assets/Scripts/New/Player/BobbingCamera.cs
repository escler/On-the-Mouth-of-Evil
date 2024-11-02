using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraPos;
    private float _timer, _defaultPosY, _defaultPosX, _defaultPosYInitialPos, _timerReset;
    [SerializeField] float bobbingSpeed, bobbingAmount, runBobbingSpeed, runBobbingAmount;
    private float _actualBobbingSpeed, _actualBobbingAmount;
    private bool _bobbingEnable, _run;
    private PlayerMovement _movement;

    private void Awake()
    {
        _defaultPosX = cameraPos.transform.localPosition.x;
        _defaultPosY = cameraPos.transform.localPosition.y;
    }

    private void Start()
    {
        _movement = PlayerHandler.Instance.movement;
    }

    void Update()
    {
        
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        _bobbingEnable = inputX != 0 || inputY != 0;
        
        _run = _movement.Run;
        _actualBobbingSpeed = _run ? runBobbingSpeed : bobbingSpeed;
        _actualBobbingAmount = _run ? runBobbingAmount : bobbingAmount;
        
    }

    private void FixedUpdate()
    {
        if (!_bobbingEnable)
        {
            //IdleBobbing();
            return;
        }
        MakeBobbing();
    }

    void MakeBobbing()
    {
        _timer += Time.deltaTime * _actualBobbingSpeed;
        cameraPos.transform.localPosition = new Vector3(cameraPos.transform.localPosition.x + Mathf.Sin(_timer) * _actualBobbingAmount * Time.deltaTime,
            cameraPos.transform.localPosition.y + Mathf.Cos(_timer) * _actualBobbingAmount / 8 * Time.deltaTime,
            cameraPos.transform.localPosition.z);
    }

    void IdleBobbing()
    {
        _timer += Time.deltaTime * _actualBobbingSpeed / 4;
        cameraPos.transform.localPosition = new Vector3(cameraPos.transform.localPosition.x,
            cameraPos.transform.localPosition.y + Mathf.Cos(_timer) * _actualBobbingAmount / 8 * Time.deltaTime,
            cameraPos.transform.localPosition.z);
    }
}
