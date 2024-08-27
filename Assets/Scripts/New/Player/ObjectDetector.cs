using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    public LayerMask layer;
    public Transform _cameraPos;
    public int distance;

    private void Awake()
    {
        //_cameraPos = Camera.main.transform;
    }

    private void Update()
    {
        RaycastHit hit;
        bool ray = Physics.Raycast(_cameraPos.position, _cameraPos.forward, out hit, distance, layer);
        
        if (ray && Input.GetButtonDown("Interact"))
        {
            hit.transform.GetComponent<Item>().OnGrabItem();
        }
    }
}
