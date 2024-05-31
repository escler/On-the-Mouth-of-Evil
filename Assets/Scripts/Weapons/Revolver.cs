using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : RangedWeapon
{
    
    private void Update()
    {
        OnUpdate();
    }

    protected override void Aim()
    {

    }

    protected override void Shoot()
    {
        var dir = targetAim.position - cameraPos.position;
        RaycastHit hit;
        var ray = Physics.Raycast(cameraPos.position, dir, out hit, layerMask);
        actualCd = fireRate;
        
        if (ray)
        {
            var target = hit.transform;
            _weaponFeedback.WeaponShootFeedback(hit.point, target.gameObject.layer, hit.normal);
            if (target.gameObject.layer == 7)
            {
                target.GetComponentInParent<LifeHandler>().OnTakeDamage(damage);
            }
        }
    }
}
