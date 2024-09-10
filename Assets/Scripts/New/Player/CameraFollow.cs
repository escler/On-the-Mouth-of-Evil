using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance { get; private set; }
    
    [SerializeField] private Transform cameraPos;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

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

    public void SetNewCameraPos(Transform pos)
    {
        cameraPos = pos;
    }
}
