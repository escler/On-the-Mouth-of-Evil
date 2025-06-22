using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailData : MonoBehaviour
{
    public string emailText;
    private Button _btn;

    private void OnEnable()
    {
        _btn = GetComponent<Button>();
        
        _btn.onClick.AddListener(OpenAndModifyText);
    }

    private void OnDestroy()
    {
        _btn.onClick.RemoveAllListeners();
    }

    private void OnDisable()
    {
        _btn.onClick.RemoveAllListeners();
    }

    private void OpenAndModifyText()
    {
        GetComponentInParent<MailList>().ChangeText(emailText);
    }
}
