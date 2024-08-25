using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BanishEnergyUI : MonoBehaviour
{
    private Slider _slider;
    private bool _lerpingEnergy;
    private float _timeScale;
    public GameObject keyUI;
    [SerializeField] private PlayerEnergyHandler _energyHandler;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        StartCoroutine(BindMethods());
    }

    private void OnDestroy()
    {
        _energyHandler.OnEnergyChange -= ChangeValue;
    }

    IEnumerator BindMethods()
    {
        yield return new WaitForEndOfFrame();
        _energyHandler = Player.Instance.playerEnergyHandler;
        _energyHandler.OnEnergyChange += ChangeValue;
        _slider.value = _energyHandler.ActualAmount;
    }

    private void ChangeValue()
    {
        _slider.maxValue = _energyHandler.maxAmount;

        if (_lerpingEnergy) return;
        _timeScale = 0;
        StartCoroutine(LerpEnergy());
    }
    
    private IEnumerator LerpEnergy()
    {
        float speed = 2f;
        float startHealth = _slider.value;
  
        _lerpingEnergy = true;
        while(_timeScale < 1)
        {
            _timeScale += Time.deltaTime * speed;
            _slider.value = Mathf.Lerp(startHealth, _energyHandler.ActualAmount, _timeScale);
            yield return new WaitForEndOfFrame();
        }
        _lerpingEnergy = false;

        keyUI.SetActive(_slider.value >= _slider.maxValue);
    }
    
    
}
