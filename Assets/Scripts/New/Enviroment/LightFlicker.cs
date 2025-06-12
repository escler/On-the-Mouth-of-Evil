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
    [SerializeField] private bool flickMaterial;
    [ColorUsage(true, true)][SerializeField] Color lightOnColor, lightOffColor;
    [SerializeField] MeshRenderer lightRenderer;
    MaterialPropertyBlock _props;

    private void Awake()
    {
        _lights = GetComponentsInChildren<Light>();
        if (flickMaterial)
        {
            _props = new MaterialPropertyBlock();
            _props.SetColor("_EmissionColor", lightOnColor);
            lightRenderer.SetPropertyBlock(_props);
        }
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

            if (flickMaterial)
            {
                _props.SetColor("_EmissionColor", _lights[0].enabled ? lightOnColor : lightOffColor);
                lightRenderer.SetPropertyBlock(_props);

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
        _props.SetColor("_EmissionColor", lightOnColor);
        lightRenderer.SetPropertyBlock(_props);
    }
}
