using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObject : MonoBehaviour
{
    public float secondsToHide;

    private void OnEnable()
    {
        StartCoroutine(HideGO());
    }

    IEnumerator HideGO()
    {
        yield return new WaitForSeconds(secondsToHide);
        gameObject.SetActive(false);
    }
}
