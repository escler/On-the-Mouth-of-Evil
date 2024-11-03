using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VSyncUI : MonoBehaviour
{
    private void Awake()
    {
        QualitySettings.vSyncCount = 1;
    }
}
