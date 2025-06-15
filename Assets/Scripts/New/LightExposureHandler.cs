using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightExposureHandler : MonoBehaviour
{
    public static LightExposureHandler Instance {get; private set;}
    [SerializeField] private Volume volume;
    [SerializeField] ColorAdjustments colorAdjustments;

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
        if(volume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.postExposure.overrideState = true;
        }

        if (colorAdjustments == null)
        {
            print("No encontre el color adjustments");
            yield break;
        }
        
        if (PlayerPrefs.HasKey("LightExposure"))
        {
            colorAdjustments.postExposure.value = PlayerPrefs.GetFloat("LightExposure");
        }
    }

    public void ChangeLightExposure(float exposure)
    {
        colorAdjustments.postExposure.value = exposure;
        PlayerPrefs.SetFloat("LightExposure", exposure);
        PlayerPrefs.Save();
    }
}
