using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySelectorUI : MonoBehaviour
{
    public static InventorySelectorUI Instance { get; private set; }
    public GameObject[] directions;
    private int actual;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        actual = 0;
    }

    public void OnChangeSelection()
    {
        directions[actual].SetActive(false);
        actual = Inventory.Instance.countSelected;
        directions[actual].SetActive(true);
    }
}
