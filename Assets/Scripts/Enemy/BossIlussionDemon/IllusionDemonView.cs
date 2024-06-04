using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemonView : MonoBehaviour
{
    public GameObject slashVFX;
    public Transform weaponTransform;

    private void Start()
    {
        
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
