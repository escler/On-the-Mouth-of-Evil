using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public GameObject menu;

    private bool _cursorVisibleState;
    private CursorLockMode _cursorLockModeState;
    [SerializeField] private Button backButton;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManagerNew.Instance.cantPause) return;
            SwitchMenu();
        }
    }

    private void Awake()
    {
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(SwitchMenu);
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveAllListeners();
    }

    public void SwitchMenu()
    {
        print("Switching menu");
        menu.SetActive(!menu.activeInHierarchy);
        if (menu.activeInHierarchy)
        {
            PauseState();
            EventSystem.current.SetSelectedGameObject(null);
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
