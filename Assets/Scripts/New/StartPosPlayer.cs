using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosPlayer : MonoBehaviour
{
    private void Awake()
    {
        PlayerHandler.Instance.transform.position = transform.position;
        PlayerHandler.Instance.transform.rotation = transform.rotation;
    }
}
