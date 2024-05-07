using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyHandler : MonoBehaviour
{
    public static KeyHandler Instance { get; set; }

    private List<Key> _keysInInventory;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
    }

    public void AddKey(Key keyObtained)
    {
        _keysInInventory.Add(keyObtained);
    }
    
    
}
