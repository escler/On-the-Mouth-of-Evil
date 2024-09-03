using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUI : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) gameObject.SetActive(false);
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
