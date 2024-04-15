using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : RangedWeapon
{
    private void Awake()
    {
    }

    private void Update()
    {
        OnUpdate();
    }

    protected override void Aim()
    {

    }

    protected override void Shoot()
    {
        Debug.Log("dispare");
        var dir = targetAim.position - chest.position;
        RaycastHit hit;
        var ray = Physics.Raycast(chest.position, dir, out hit, dir.magnitude, layerMask);

        if (ray)
        {
            Debug.Log("Pegue con " + hit.transform.name);
            var target = hit.transform;
            if(target.gameObject.layer == 7) target.GetComponent<LifeHandler>().OnTakeDamage(damage);
        }
    }
}
