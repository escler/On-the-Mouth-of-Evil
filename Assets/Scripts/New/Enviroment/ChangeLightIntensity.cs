using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLightIntensity : MonoBehaviour
{
    private void Awake()
    {
        RenderSettings.ambientIntensity = 0f;
        DynamicGI.UpdateEnvironment();
    }
}
