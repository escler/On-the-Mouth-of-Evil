using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveEnemy : MonoBehaviour
{
    public SkinnedMeshRenderer[] _skinnedMaterials;
    private MaterialPropertyBlock property;
    public VisualEffect VFXDissolve;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;
    private float counter;

    private void Start()
    {
        property = new MaterialPropertyBlock();
        property.SetFloat("_DissolveAmount", counter);
    }
    
    public void ActivateDissolve()
    {
        StartCoroutine(Dissolve());
    }

    IEnumerator Dissolve()
    {
        if (VFXDissolve != null) VFXDissolve.Play();
        if (_skinnedMaterials.Length <= 0) yield break;
        
        while (counter < 1)
        {
            counter += dissolveRate;

            for (int i = 0; i < _skinnedMaterials.Length; i++)
            {
                property.SetFloat("_DissolveAmount", counter);
                _skinnedMaterials[i].SetPropertyBlock(property);
            }
            yield return new WaitForSeconds(refreshRate);
        }

        transform.parent.gameObject.SetActive(false);

    }
}
