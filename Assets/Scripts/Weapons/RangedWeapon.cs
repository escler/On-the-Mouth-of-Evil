using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeapon : Weapon
{
    protected Bullet _myBullet;
    private WeaponType _weaponType = WeaponType.Ranged;

    protected virtual void Shoot()
    {
        
    }
}
