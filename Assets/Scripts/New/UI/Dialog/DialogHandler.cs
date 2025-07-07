using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DialogHandler : MonoBehaviour
{
    public static DialogHandler Instance { get; private set; }
    private TextMeshProUGUI _text;
    private float _actualTime;
    public int showTime;
    private bool _textActive;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _text.text = "";
        SceneManager.sceneLoaded += Reset;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= Reset;
    }

    public void ChangeText(string text)
    {
        _text.text = text;
        _actualTime = showTime;
        if (_textActive) return;
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        _textActive = true;
        while (_actualTime > 0)
        {
            _actualTime -= Time.deltaTime;

            yield return null;
        }
        
        _text.text = "";
        _textActive = false;
    }

    private void Reset(Scene scene, LoadSceneMode loadSceneMode)
    {
        _text.text = "";
        _textActive = false;
    }
}
