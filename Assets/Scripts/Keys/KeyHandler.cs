using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyHandler : MonoBehaviour
{
    public static KeyHandler Instance { get; set; }

    [SerializeField] private List<KeyType> _keysInInventory = new List<KeyType>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
    }

    public void AddKey(KeyType keyObtained)
    {
        _keysInInventory.Add(keyObtained);
    }
    
    
}
