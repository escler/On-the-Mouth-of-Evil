using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraPos;

    private float _timer, _defaultPosY, _defaultPosX;
    [SerializeField] private float bobbingSpeed = 8f;
    [SerializeField] private float bobbingAmount = 0.015f;
    [SerializeField] private float runBobbingSpeed = 10f;
    [SerializeField] private float runBobbingAmount = 0.025f;

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

    private void Update()
    {
        if (_movement.ritualCinematic) return;

        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        _bobbingEnable = Mathf.Abs(inputX) > 0.1f || Mathf.Abs(inputY) > 0.1f;

        _run = _movement.Run;
        _actualBobbingSpeed = _run ? runBobbingSpeed : bobbingSpeed;
        _actualBobbingAmount = _run ? runBobbingAmount : bobbingAmount;
        if (!_bobbingEnable)
        {
            ReturnToDefaultPosition();
            return;
        }

        MakeBobbing();
    }

    private void FixedUpdate()
    {

    }

    void ReturnToDefaultPosition()
    {
        Vector3 targetPos = new Vector3(_defaultPosX, _defaultPosY, cameraPos.localPosition.z);
        cameraPos.localPosition = Vector3.Lerp(cameraPos.localPosition, targetPos, Time.deltaTime * 4f);

        // Si estamos muy cerca, forzamos la posición exacta
        if (Vector3.Distance(cameraPos.localPosition, targetPos) < 0.001f)
        {
            cameraPos.localPosition = targetPos;
        }

        _timer = 0f; // evitar salto al volver a caminar
    }

    public void DoBobbing()
    {
        MakeBobbing();
    }

    void MakeBobbing()
    {
        _timer += Time.deltaTime * _actualBobbingSpeed;

        float offsetY = Mathf.Sin(_timer) * _actualBobbingAmount;
        float offsetX = Mathf.Cos(_timer * 0.5f) * (_actualBobbingAmount);

        cameraPos.localPosition = new Vector3(_defaultPosX + offsetX, _defaultPosY + offsetY, cameraPos.localPosition.z);
    }

    void IdleBobbing()
    {
        _timer += Time.deltaTime * _actualBobbingSpeed / 4;
        cameraPos.transform.localPosition = new Vector3(cameraPos.transform.localPosition.x,
            cameraPos.transform.localPosition.y + Mathf.Cos(_timer) * _actualBobbingAmount / 8 * Time.deltaTime,
            cameraPos.transform.localPosition.z);
    }
}
