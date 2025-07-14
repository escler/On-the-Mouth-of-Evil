using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailUI : MonoBehaviour
{
    [SerializeReference] private GameObject newMail, mailGO;
    [SerializeReference] private Button mailButton;
    AudioSource _audioSource;

    private void Awake()
    {
        mailButton.onClick.AddListener(OpenMail);
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(WaitCor());
    }

    IEnumerator WaitCor()
    {
        yield return new WaitForSeconds(0.1f);
        var mail = MailHandler.Instance.GetEmailLog();
        var actual = PlayerPrefs.HasKey("MailCount") ? PlayerPrefs.GetInt("MailCount") : 0;

        if (actual >= mail.Count) yield break;
        newMail.SetActive(true);
        _audioSource.Play();
        PlayerPrefs.SetInt("MailCount", mail.Count);
    }

    private void OnDestroy()
    {
        mailButton.onClick.RemoveAllListeners();
    }

    private void OpenMail()
    {
        PCHandler.Instance.clickSound.Play();
        newMail.SetActive(false);
        mailGO.SetActive(true);
    }

    public void NewMessage()
    {
        newMail.SetActive(true);
    }
    
    
}
