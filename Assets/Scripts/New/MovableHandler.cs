using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableHandler : MonoBehaviour
{
    public static MovableHandler Instance { get; private set; }
    
    public List<MovableItem> movablesItems;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
