using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightFlicker : MonoBehaviour, IInteractableEnemy
{
    private Light[] _lights;
    private bool _flicker;
    private float minInt = 0.025f;
    private float maxInt = 0.5f;
    private float _interval;

    private void Awake()
    {
        _lights = GetComponentsInChildren<Light>();
        _flicker = true;
    }

    public void OnStartInteract()
    {
        _flicker = true;
    }

    private void Update()
    {
        if (!_flicker) return;

        if (_interval < 0)
        {
            _interval = Random.Range(minInt, maxInt);
            foreach (var light in _lights)
            {
                light.enabled = !light.enabled;
            }
        }

        _interval -= Time.deltaTime;
        
    }

    public void OnEndInteract()
    {
        _flicker = false;
        foreach (var light in _lights)
        {
            light.enabled = true;
        }
    }
}
