using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeFireBall : MonoBehaviour
{
    public GameObject fireBall;
    public float duration;
    private float _scalePertime;

    private void OnEnable()
    {
        _scalePertime = 1 / duration;
    }

    private void Update()
    {
        transform.localScale += Vector3.one * (_scalePertime * Time.deltaTime);
    }

    private void OnDisable()
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }
}
