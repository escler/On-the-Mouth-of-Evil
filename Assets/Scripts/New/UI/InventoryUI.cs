using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddItem(Item i)
    {
        var uiElement = Instantiate(i.uiElement);
        uiElement.transform.SetParent(transform);
        uiElement.transform.localScale = Vector3.one;
    }
}
