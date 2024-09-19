using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorduraHandler : MonoBehaviour
{
    public static CorduraHandler Instance { get; private set; }
    
    public Material corduraMaterial;
    private bool _corduraOn, _inverseCordura;
    private float _corduraMin = -1;
    private float _corduraMax = 1;
    private float _actualCordura;
    private float _targetCordura;
    private float _interval = .01f;

    public bool CorduraOn
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
    }

    public void StartCordura()
    {
        StartCoroutine(CorduraOnCor());
    }

    
    IEnumerator CorduraOnCor()
    {
        while (_corduraOn)
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
