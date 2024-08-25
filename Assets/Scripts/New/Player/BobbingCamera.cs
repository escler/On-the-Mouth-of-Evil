using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraPos;
    private float _timer, _defaultPosY, _defaultPosX, _defaultPosYInitialPos, _timerReset;
    [SerializeField] float boobingSpeed, bobbingAmount;

    private void Awake()
    {
        _defaultPosX = cameraPos.transform.localPosition.x;
        _defaultPosY = cameraPos.transform.localPosition.y;
    }

    void Update()
    {

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        if (inputX != 0 || inputY != 0)
        {
            _timer += Time.deltaTime * boobingSpeed;
            cameraPos.transform.localPosition = new Vector3(cameraPos.transform.localPosition.x + Mathf.Cos(_timer) * bobbingAmount * Time.deltaTime, _defaultPosY, cameraPos.transform.localPosition.z);
        }
        else
        {
            cameraPos.transform.localPosition = new Vector3(Mathf.Lerp(cameraPos.transform.localPosition.x, _defaultPosX, Time.deltaTime * boobingSpeed), Mathf.Lerp(cameraPos.transform.localPosition.y, _defaultPosY, Time.deltaTime * boobingSpeed), cameraPos.localPosition.z);
        }
    }
}
