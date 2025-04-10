using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour, IInteractable
{
    public static Oven Instance { get; private set; }
    public ParticleSystem ps;
    public ParticleSystem.EmissionModule emission;
    private bool _on;
    public AudioSource _hornilla;
    [SerializeField] private Knob[] _knobs;
    [SerializeField] private ParticleSystem[] _particles;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        foreach (var p in _particles)
        {
            var ps = p.emission;
            ps.enabled = false;
        }
    }

    public void OnInteractItem()
    {
        _on = !_on;
        if (_on)
        {
            _hornilla.Play();

        }
        else
        {
            _hornilla.Stop();
        }
        emission.enabled = _on;
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public void OnInteractWithObject()
    {
    }

    public string ShowText()
    {
        return "";
    }

    public bool CanShowText()
    {
        return true;
    }

    public void ChangeFireState(Knob knob)
    {
        for (int i = 0; i < _knobs.Length; i++)
        {
            if(_knobs[i] != knob) continue;

            var ps = _particles[i].emission;
            ps.enabled = !ps.enabled;
            break;
        }
    }
    
}
