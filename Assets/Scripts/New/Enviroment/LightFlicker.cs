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
    public AudioSource _flickerSound;
    public AudioSource _light;

    private void Awake()
    {
        _lights = GetComponentsInChildren<Light>();
    }

    public void OnStartInteract()
    {
        _light.Stop();
        _flicker = true;
        _flickerSound.Play();
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
        _flickerSound.Stop();
        _light.Play();
        foreach (var light in _lights)
        {
            light.enabled = true;
        }
    }
}
