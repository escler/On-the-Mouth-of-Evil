using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillDataUI : MonoBehaviour
{
    public TextMeshProUGUI _textMeshPro;
    private BookSkillTrigger _bookSkillTrigger;
    public float secondsToHideInfo;

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        StartCoroutine(BindMethods());
    }

    private void OnDestroy()
    {
        _bookSkillTrigger.OnSkillActivate -= ShowInfo;
    }

    IEnumerator BindMethods()
    {
        yield return new WaitForEndOfFrame();
        _bookSkillTrigger = Player.Instance.GetComponent<BookSkillTrigger>();
        _bookSkillTrigger.OnSkillActivate += ShowInfo;
    }

    private void ShowInfo()
    {
        _textMeshPro.text = "Skill Damage Done:\n" + _bookSkillTrigger.damageDone;
        StartCoroutine(HideInfo());
    }

    IEnumerator HideInfo()
    {
        yield return new WaitForSeconds(secondsToHideInfo);
        _textMeshPro.text = "";
    }
}
