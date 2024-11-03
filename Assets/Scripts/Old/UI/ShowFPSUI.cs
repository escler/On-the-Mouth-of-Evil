using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowFPSUI : MonoBehaviour
{
    private int _active;
    private Toggle _toggle;

    private void OnEnable()
    {
        _toggle = GetComponent<Toggle>();
    }

    void SetToggle()
    {
    }

    public void ChangeValue(bool isOn)
    {
        _active = isOn ? 1 : 0;
        PlayerPrefs.SetInt("ShowFPS", _active);
        PlayerPrefs.Save();
    }
}
