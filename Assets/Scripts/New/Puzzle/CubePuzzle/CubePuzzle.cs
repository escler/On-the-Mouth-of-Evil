using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePuzzle : MonoBehaviour
{
    public static CubePuzzle Instance { get; private set; }
    private int[] positionCode = new int[4];
    [SerializeField] private string code;
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
