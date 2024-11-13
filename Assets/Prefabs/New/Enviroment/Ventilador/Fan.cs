using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour, IInteractable
{
    private float _speed;
    private Animator _animator;
    private bool _animationRun;
    public GameObject aspas1, aspas2;
    public float speedFan;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _speed = 0;
        _animator.speed = _speed;
    }

    // Update is called once per frame
    void Update()
    {
        aspas1.transform.Rotate(0, 0, _speed * Time.deltaTime * speedFan);
        aspas2.transform.Rotate(0, 0, -_speed * Time.deltaTime * speedFan);
    }

    public void OnInteractItem()
    {
        _animationRun = !_animationRun;
        StopAllCoroutines();
        StartCoroutine(_animationRun ? SpeedUp() : SpeedSlow());
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public void OnInteractWithObject()
    {
    }

    public string ShowText()
    {
        return "";
    }

    public bool CanShowText()
    {
        return true;
    }

    IEnumerator SpeedUp()
    {
        var actualSpeed = _speed;
        float ticks = 0;

        while (ticks < 1)
        {
            ticks += Time.deltaTime;
            _speed = Mathf.Lerp(actualSpeed, 1, ticks);
            _animator.speed = _speed;
            yield return null;
        }
    }

    IEnumerator SpeedSlow()
    {
        var actualSpeed = _speed;
        float ticks = 0;

        while (ticks < 1)
        {
            ticks += Time.deltaTime;
            _speed = Mathf.Lerp(actualSpeed, 0, ticks);
            _animator.speed = _speed;
            yield return null;
        }
    }
}
