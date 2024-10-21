using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    public GameObject menu;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchMenu();
        }
    }

    private void SwitchMenu()
    {
        menu.SetActive(!menu.activeInHierarchy);
        Cursor.visible = menu.activeInHierarchy;
        if (Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.Confined;
            PlayerHandler.Instance.UnPossesPlayer();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            PlayerHandler.Instance.PossesPlayer();
        }
    }
}
