using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualManagerLevel2 : MonoBehaviour
{
    public static RitualManagerLevel2 Instance {get; private set;}
    public GameObject[] levitatingItems;


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
