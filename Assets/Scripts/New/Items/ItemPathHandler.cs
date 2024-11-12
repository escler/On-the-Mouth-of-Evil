using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPathHandler : MonoBehaviour
{
    public static ItemPathHandler Instance { get; private set; }
    
    public GameObject[] goodPathObjs;
    public GameObject[] badPathObjs;

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
