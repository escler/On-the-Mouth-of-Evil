using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossLifeHandlerUI : MonoBehaviour
{
    private Slider _slider;
    private bool _lerpingHealth;
    private float _timeScale;
    [SerializeField] private IllusionDemonLifeHandler _lifeHandler;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        _slider = GetComponent<Slider>();
        _lifeHandler.OnLifeChange += ChangeValue;
        _lifeHandler.GetComponent<IllusionDemon>().OnBossDefeated += HideUI;
        ChangeValue();
    }

    private void OnDestroy()
    {
        _lifeHandler.GetComponent<IllusionDemon>().OnBossDefeated -= HideUI;
        _lifeHandler.OnLifeChange -= ChangeValue;
    }

    private void HideUI()
    {
        gameObject.SetActive(false);
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
