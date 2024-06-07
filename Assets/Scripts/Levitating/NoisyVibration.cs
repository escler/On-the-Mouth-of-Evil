using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//FINAL - Caamaño Romina - movimiento vibratorio independiente
public class NoisyVibration : MonoBehaviour
{
    [SerializeField] private Vector3 dir = Vector3.right;
    [SerializeField] private float amp = 1f;
    [SerializeField] private float freq = 1f;

    private float t = 0f;
    private Vector3 origin;

    private void Awake()
    {
        origin = transform.localPosition;
    }

    private void Update()
    {
        t += Time.deltaTime;
        transform.localPosition = origin + dir.normalized * amp * Mathf.Sin(2f * Mathf.PI * freq * t);
    }
}
