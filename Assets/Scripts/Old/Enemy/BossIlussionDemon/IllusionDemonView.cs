using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemonView : MonoBehaviour
{
    public GameObject slashVFX;
    public Transform weaponTransform;
    public GameObject lifeUI;

    private void OnEnable()
    {
        lifeUI.SetActive(true);
    }

    public void SlashVFX()
    {
        var newSlash = Instantiate(slashVFX);
        newSlash.transform.position = weaponTransform.position;
        newSlash.transform.rotation = weaponTransform.rotation;
        
    }

    public void StopTrail()
    {
        //slashVFX.SetActive(false);
    }
}
