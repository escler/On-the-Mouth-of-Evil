using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform cameraPos;

    private void Start()
    {
        if (cameraPos != null) return;

        cameraPos = PlayerHandler.Instance.cameraPos;
    }

    private void LateUpdate()
    {
        if (cameraPos == null) cameraPos = PlayerHandler.Instance.cameraPos;
        transform.position = cameraPos.position;
        transform.rotation = cameraPos.rotation;
    }
}
