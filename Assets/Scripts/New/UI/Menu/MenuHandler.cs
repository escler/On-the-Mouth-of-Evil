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

    public void SwitchMenu()
    {
        menu.SetActive(!menu.activeInHierarchy);
        Cursor.visible = menu.activeInHierarchy;
        Cursor.lockState = Cursor.visible ? CursorLockMode.Confined : CursorLockMode.Locked;
        Time.timeScale = Cursor.visible ? 0 : 1;
    }
}
