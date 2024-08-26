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
        
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        _bobbingEnable = inputX != 0 || inputY != 0;

        if (_bobbingEnable)
        {
            _run = _movement.Run;
            _actualBobbingSpeed = _run ? runBobbingSpeed : bobbingSpeed;
            _actualBobbingAmount = _run ? runBobbingAmount : bobbingAmount;
            MakeBobbing();
        }
        else ResetCamera();
    }

    void MakeBobbing()
    {
        _timer += Time.deltaTime * _actualBobbingSpeed;
        cameraPos.transform.localPosition = new Vector3(cameraPos.transform.localPosition.x + Mathf.Sin(_timer) * _actualBobbingAmount * Time.deltaTime,
            cameraPos.transform.localPosition.y + Mathf.Cos(_timer) * _actualBobbingAmount / 8 * Time.deltaTime,
            cameraPos.transform.localPosition.z);
    }

    void ResetCamera()
    {
        cameraPos.transform.localPosition = new Vector3(Mathf.Lerp(cameraPos.transform.localPosition.x, _defaultPosX, Time.deltaTime * _actualBobbingSpeed),
            Mathf.Lerp(cameraPos.transform.localPosition.y, _defaultPosY, Time.deltaTime * _actualBobbingSpeed),
            cameraPos.localPosition.z);
    }
}
