using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance { get; private set; }
    
    [SerializeField] private Transform cameraPos;
    public bool inRitual;

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
        transform.position = cameraPos.position;
        transform.rotation = cameraPos.rotation;
    }

    private void Update()
    {
        if (cameraPos == null) cameraPos = inRitual ? CameraCinematicHandler.Instance.ritual : PlayerHandler.Instance.cameraPos;

        if (inRitual)
        {
            transform.position = Vector3.Lerp(transform.position,cameraPos.position,Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation,cameraPos.rotation, Time.deltaTime);
        }
        else
        {
            transform.position = cameraPos.position;
            transform.rotation = cameraPos.rotation;
        }
    }

    public void SetNewCameraPos(Transform pos)
    {
        cameraPos = pos;
    }
}
