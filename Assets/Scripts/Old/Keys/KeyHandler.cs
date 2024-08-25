using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyHandler : MonoBehaviour
{
    public static KeyHandler Instance { get; private set; }

    private Dictionary<KeyType, string> _keysInInventory = new Dictionary<KeyType, string>();

    public Dictionary<KeyType, string> KeysInInventory => _keysInInventory;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void AddKey(KeyType keyObtained, string name)
    {
        _keysInInventory.Add(keyObtained, name);
        KeysUIAdquired.Instance.AddText(name);
    }
    
}
