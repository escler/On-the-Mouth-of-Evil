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
        var dir = targetAim.position - camera.position;
        RaycastHit hit;
        var ray = Physics.Raycast(camera.position, dir, out hit, layerMask);
        actualCd = fireRate;
        
        if (ray)
        {
            var target = hit.transform;
            _weaponFeedback.WeaponShootFeedback(hit.point, target.gameObject.layer, hit.normal);
            if (target.gameObject.layer == 7)
            {
                target.GetComponent<LifeHandler>().OnTakeDamage(damage);
            }
        }
    }

    public void Reload() => Reload();

    private void OnDrawGizmos()
    {
        //Gizmos.DrawRay(chest.position, targetAim.position - chest.position);
    }
}
