using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TropiesHandler : MonoBehaviour
{
    public static TropiesHandler Instance { get; private set; }
    public GameObject[] goodThropies, badThropies;

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
