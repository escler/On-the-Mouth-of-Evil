using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZoneManager : MonoBehaviour
{
    public static BossZoneManager Instance { get; private set; }

    public Transform[] points;
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
