using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCinematicHandler : MonoBehaviour
{
    public static CameraCinematicHandler Instance { get; private set; }
    public Transform ritual;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        transform.LookAt(ritual);
    }
}
