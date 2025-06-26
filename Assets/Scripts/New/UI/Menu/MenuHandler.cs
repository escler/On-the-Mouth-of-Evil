using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    public GameObject menu;

    private bool _cursorVisibleState;
    private CursorLockMode _cursorLockModeState;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManagerNew.Instance.cantPause) return;
            SwitchMenu();
        }
    }

    public void SwitchMenu()
    {
        menu.SetActive(!menu.activeInHierarchy);
        if (menu.activeInHierarchy)
        {
            PauseState();
        }
        else
        {
            UnPauseState();
        }
    }

    private void PauseState()
    {
        _cursorVisibleState = Cursor.visible;
        _cursorLockModeState = Cursor.lockState;
        print(_cursorVisibleState);
        print(_cursorLockModeState);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
    }

    private void UnPauseState()
    {
        Cursor.visible = _cursorVisibleState;
        Cursor.lockState = _cursorLockModeState;
        Time.timeScale = 1;
    }
}
