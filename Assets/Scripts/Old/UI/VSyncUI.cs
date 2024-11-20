using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VSyncUI : MonoBehaviour
{
    private void Start()
    {
        QualitySettings.vSyncCount = 1;
    }
}
