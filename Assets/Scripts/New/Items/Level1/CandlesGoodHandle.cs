using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandlesGoodHandle : MonoBehaviour
{
    [SerializeField] private BoxCollider box;
    [SerializeField] Candle[] candles;
    [SerializeField] private Animator doorAnimator;
    public void OpenDoor()
    {
        box.enabled = false;
        gameObject.SetActive(false);
        doorAnimator.SetBool("Open", true);
        foreach (var c in candles)
        {
            c.canTake = true;
            c.canShowText = true;
        }
    }
}
