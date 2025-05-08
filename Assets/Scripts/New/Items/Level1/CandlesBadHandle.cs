using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandlesBadHandle : MonoBehaviour
{
    [SerializeField] private BoxCollider box;
    [SerializeField] private Candle[] candles;
    [SerializeField] Animator doorAnimator;
    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponentInParent<AudioSource>();
    }

    public void OpenDoor()
    {
        _audioSource.Play();
        box.enabled = false;
        doorAnimator.SetBool("Open",true);
        foreach (var c in candles)
        {
            c.canTake = true;
            c.canShowText = true;
        }

        gameObject.SetActive(false);
    }
}
