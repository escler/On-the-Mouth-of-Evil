using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorduraHandler : MonoBehaviour
{
    public static CorduraHandler Instance { get; private set; }
    
    public Material corduraMaterial;
    private bool _inverseCordura, _corduraActivate;
    private float _corduraOn;
    private float _corduraMin = -.65f;
    private float _corduraMax = .65f;
    private float _actualCordura;
    private float _targetCordura;
    private float _actualVignette;
    private float _targetVignette = 67.3f;
    private float _interval = .01f;

    public float CorduraOn
    {
        get => _corduraOn;
        set => _corduraOn = value;
    }
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
        corduraMaterial.SetFloat("_TwirlStrength", 0);
        corduraMaterial.SetFloat("_intensity", 0);
    }
    

    public void StartCordura()
    {
        _corduraOn = 10f;
        if (_corduraActivate) return;
        _corduraActivate = true;
        StartCoroutine(CorduraOnCor());
        StartCoroutine(VignneteCor());
    }

    IEnumerator VignneteCor()
    {
        while (_corduraOn > 0)
        {
            _corduraOn -= 0.01f;
            if (_actualVignette < _targetVignette)
            {
                _actualVignette += .5f;
                _actualVignette = Mathf.Clamp(_actualVignette, 0, _targetVignette);
                corduraMaterial.SetFloat("_intensity", _actualVignette);
            }
            yield return new WaitForSeconds(0.01f);
        }
        
        StartCoroutine(ResetVignnete());
    }

    IEnumerator ResetVignnete()
    {
        while (_actualVignette > 0)
        {
            _actualVignette -= .5f;
            _actualVignette = Mathf.Clamp(_actualVignette, 0, _targetVignette);
            corduraMaterial.SetFloat("_intensity", _actualVignette);
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    IEnumerator CorduraOnCor()
    {
        while (_corduraOn > 0)
        {
            if (_actualCordura == _targetCordura)
            {
                _inverseCordura = !_inverseCordura;
                _targetCordura = _inverseCordura ? _corduraMin : _corduraMax;
                _interval *= -1;
            }

            _actualCordura += _interval;
            _actualCordura = Mathf.Clamp(_actualCordura, _corduraMin, _corduraMax);
            corduraMaterial.SetFloat("_TwirlStrength", _actualCordura);
            yield return new WaitForSeconds(.01f);
        }

        StartCoroutine(ResetCordura());
    }

    IEnumerator ResetCordura()
    {
        _corduraActivate = false;
        while (Mathf.Abs(_actualCordura) >= .01f)
        {
            _interval = _actualCordura < 0 ? Mathf.Abs(_interval) : Mathf.Abs(_interval) * -1;
            _actualCordura += _interval;
            _actualCordura = Mathf.Clamp(_actualCordura, _corduraMin, _corduraMax);
            corduraMaterial.SetFloat("_TwirlStrength", _actualCordura);
            yield return new WaitForSeconds(.01f);
        }
        
        corduraMaterial.SetFloat("_TwirlStrength", 0);
    }
}
