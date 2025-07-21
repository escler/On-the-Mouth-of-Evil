using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightExposureHandler : MonoBehaviour
{
    public static LightExposureHandler Instance {get; private set;}

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        StartCoroutine(GetValue());

    }

    IEnumerator GetValue()
    {
        yield return null;

        RenderSettings.ambientIntensity =
            PlayerPrefs.HasKey("LightExposure") ? PlayerPrefs.GetFloat("LightExposure") : .3f;
        
        PlayerPrefs.Save();
    }

    public void ChangeLightExposure(float exposure)
    {
        RenderSettings.ambientIntensity = exposure;
        PlayerPrefs.SetFloat("LightExposure", exposure);
        PlayerPrefs.Save();
        if (HypnosisHandler.Instance == null) return;
        HypnosisHandler.Instance.ChangeIntensity(exposure);
    }
}
