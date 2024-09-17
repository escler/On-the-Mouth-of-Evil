using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUI : MonoBehaviour
{
    private void Update()
    {
    }

    private void OnEnable()
    {
        PlayerHandler.Instance.UnPossesPlayer();
    }

    private void OnDisable()
    {
        PlayerHandler.Instance.PossesPlayer();
    }
}
