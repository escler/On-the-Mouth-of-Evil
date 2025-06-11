using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmAround : MonoBehaviour
{
    [SerializeField] private float duration;
    private float _actualTime;
    private void Awake()
    {
        _actualTime = duration;
    }

    private void Update()
    {
        _actualTime -= Time.deltaTime;

        if (_actualTime > 0) return;
        
        Destroy(gameObject);
    }
}
