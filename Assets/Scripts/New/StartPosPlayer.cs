using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosPlayer : MonoBehaviour
{
    private void Awake()
    {
        if (PlayerHandler.Instance == null) return;
        PlayerHandler.Instance.transform.position = transform.position;
        PlayerHandler.Instance.playerCam.ResetVar(transform.eulerAngles);
    }
}
