using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyUIGenerator : MonoBehaviour
{
    public GameObject keyGo;
    private GameObject _panelKeyUI;
    public Color normalColor, pressColor;
    public GameObject timerUI;
    
    public static KeyUIGenerator Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        _panelKeyUI = GameObject.FindGameObjectWithTag("PanelKeyUI");
        HidePanel();
    }

    public void AddKey(string key)
    {
        var newKey = Instantiate(keyGo);
        newKey.GetComponentInChildren<TextMeshProUGUI>().text = key.ToUpper();
        newKey.transform.SetParent(_panelKeyUI.transform);
        newKey.transform.localScale = new Vector3(1, 1, 1);
    }

    public void DeleteKeys()
    {
        var count = _panelKeyUI.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            var child = _panelKeyUI.transform.GetChild(i);
            Destroy(child.gameObject);
        }

        HidePanel();
    }

    public void ShowPanel()
    {
        _panelKeyUI.SetActive(true);
        timerUI.SetActive(true);
    }
    
    public void HidePanel()
    {
        _panelKeyUI.SetActive(false);
        timerUI.SetActive(false);
    }

    public void ChangeColor(int count)
    {
        _panelKeyUI.transform.GetChild(count).GetComponent<RawImage>().color = pressColor;
        if (count == 0) return;
        _panelKeyUI.transform.GetChild(count-1).GetComponent<RawImage>().color = normalColor;
    }
}
