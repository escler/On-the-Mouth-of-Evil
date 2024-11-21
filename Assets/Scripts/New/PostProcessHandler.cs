using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessHandler : MonoBehaviour
{
    public Volume global;
    public VolumeParameter<float> endExposure;
    private VolumeParameter<float> actual;
    public static PostProcessHandler Instance { get; private set; }
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        global.profile.TryGet(out ColorAdjustments colorAdjustments);

        if (colorAdjustments == null) return;

        colorAdjustments.postExposure.SetValue(actual);
    }

    public void IncreaseExposure()
    {
        StartCoroutine(IncreaseExposureCor());
    }

    IEnumerator IncreaseExposureCor()
    {
        global.profile.TryGet(out ColorAdjustments colorAdjustments);
        while (actual.value < 2)
        {
            actual.value += .1f;
            colorAdjustments.postExposure.SetValue(actual);
            yield return 0.01f;
        }
    }


}
