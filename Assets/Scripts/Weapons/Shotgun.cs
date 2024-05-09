using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : RangedWeapon
{
    private float[] _spreadDirX = { 0, 1f, 1.5f, 1, 0,-1, -1.5f, -1f };
    private float[] _spreadDirY = { .8f, .4f, 0, -.4f, -.8f, -.4f, 0, .5f };
    
    // Update is called once per frame
    void Update()
    {
        OnUpdate();
    }
    
    protected override void Aim()
    {
        
    }

    public override void ObtainedBullet()
    {
        MaxBullets = AmmoHandler.Instance.ShotgunBullets;
    }
    
    protected override void Shoot()
    {
        actualCd = fireRate;
        
        for (int i = 0; i < _spreadDirX.Length; i++)
        {
            
            var targetPos = targetAim.position;
            targetPos.y += _spreadDirY[i];
            targetPos.x += _spreadDirX[i];
            
            var dir = targetPos - cameraPos.position;
            RaycastHit hit;
            
            var ray = Physics.Raycast(cameraPos.position, dir, out hit, layerMask);
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

}
