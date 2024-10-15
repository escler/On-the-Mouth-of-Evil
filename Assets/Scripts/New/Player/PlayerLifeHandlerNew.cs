using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLifeHandlerNew : MonoBehaviour
{
    public static PlayerLifeHandlerNew Instance { get; private set; }
    public int startLife;
    private int _actualLife;
    public delegate void UpdateLifeUI();
    public event UpdateLifeUI OnLifeChange;

    public int ActualLife => _actualLife;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        SceneManager.sceneLoaded += SetLife;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SetLife;
    }

    private void SetLife(Scene scene, LoadSceneMode loadSceneMode)
    {
        _actualLife = startLife;
    }

    public void DamageTaked(int damage)
    {
        _actualLife -=damage;
        _actualLife = Mathf.Clamp(_actualLife, 0, startLife);
        OnLifeChange?.Invoke();
    }
}
