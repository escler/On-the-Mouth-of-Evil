using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarUI : MonoBehaviour
{
    private Slider _slider;
    private int _actualLife, _maxLife;
    private PlayerLifeHandler _lifeHandler;
    private float _timeScale;
    private bool _lerpingHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponent<Slider>();
        _lifeHandler = Player.Instance.GetComponent<PlayerLifeHandler>();
        _lifeHandler.OnLifeChange += ChangeValue;
        ChangeValue();
    }

    private void OnDestroy()
    {
        _lifeHandler.OnLifeChange -= ChangeValue;
    }

    private void ChangeValue()
    {
        _slider.maxValue = _lifeHandler.initialLife;
        
        if (_lerpingHealth) return;
        _timeScale = 0;
        StartCoroutine(LerpHealth());
    }
    
    private IEnumerator LerpHealth( )
    {
        float speed = 2f;
        float startHealth = _slider.value;
  
        _lerpingHealth = true;
        while(_timeScale < 1)
        {
            _timeScale += Time.deltaTime * speed;
            _slider.value = Mathf.Lerp(startHealth, _lifeHandler.ActualLife, _timeScale);
            yield return new WaitForEndOfFrame();
        }
        _lerpingHealth = false;
    }
}
