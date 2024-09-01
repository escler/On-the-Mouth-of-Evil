using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler Instance { get; private set; }

    public PlayerMovement movement;
    public Transform handPivot, cameraPos;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        Instance = this;
        movement = GetComponent<PlayerMovement>();
    }
    
    
}
