using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlickerLightMenu : MonoBehaviour
{
    public AnimationCurve curve;
    private Light _light;
    private float minInt = 0.025f;
    private float maxInt = 3f;
    private float minIntensity = 1f;
    private float maxIntensity = 30f;
    private float intensity;
    private float currentIntensity;
    private float _interval;
    // Start is called before the first frame update
    private void Awake()
    {
        _light = GetComponent<Light>();
        _light.intensity = 30;
        intensity = _light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        _light.intensity = curve.Evaluate(Time.time);
    }
}
