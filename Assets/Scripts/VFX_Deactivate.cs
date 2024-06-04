using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Deactivate : MonoBehaviour
{
    public float lifetime;

    private void OnEnable()
    {
        StartCoroutine("DisableObject");
    }

    private void OnDisable()
    {
        StopCoroutine("DisableObject");
    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(lifetime);
        gameObject.SetActive(false);
    }
}
