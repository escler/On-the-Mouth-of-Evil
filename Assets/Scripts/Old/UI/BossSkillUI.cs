using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSkillUI : MonoBehaviour
{
    private Slider _slider;
    private bool _lerping;
    private float _timeScale;

    private void OnEnable()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue = Player.Instance.bossSkill.cdForSkill;
        Player.Instance.bossSkill.OnSkillActivate += ChangeValue;
    }

    private void OnDestroy()
    {
        Player.Instance.bossSkill.OnSkillActivate -= ChangeValue;
    }

    private void ChangeValue()
    {
        if (_lerping) return;
        _timeScale = 0;
        StartCoroutine(LerpEnergy());
    }
    
    private IEnumerator LerpEnergy()
    {
        float speed = 2f;
        float startHealth = _slider.value;
  
        _lerping = true;
        while(_timeScale < 1)
        {
            _timeScale += Time.deltaTime * speed;
            _slider.value = Mathf.Lerp(startHealth, Player.Instance.bossSkill.actualTime, _timeScale);
            yield return new WaitForEndOfFrame();
        }
        _lerping = false;
    }
}
