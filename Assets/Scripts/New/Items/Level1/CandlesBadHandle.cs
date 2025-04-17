using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandlesBadHandle : MonoBehaviour
{
    [SerializeField] private BoxCollider box;
    [SerializeField] private Candle[] candles;
    [SerializeField] Animator doorAnimator;
    public void OpenDoor()
    {
        box.enabled = false;
        gameObject.SetActive(false);
        doorAnimator.SetBool("Open",true);
        foreach (var c in candles)
        {
            c.canTake = true;
            c.canShowText = true;
        }
    }
}
