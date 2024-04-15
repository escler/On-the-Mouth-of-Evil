using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public abstract class RangedWeapon : Weapon
{
    protected Bullet _myBullet;
    private WeaponType _weaponType = WeaponType.Ranged;
    protected bool _aiming;
    protected CinemachineFreeLook _cmf;

    private void Start()
    {
        _cmf = FindObjectOfType<CinemachineFreeLook>();
    }

    protected void OnUpdate()
    {
        _aiming = Input.GetMouseButton(1);
        if (_aiming)
        { 
            _cmf.GetComponent<CameraMovement>().SetCameraMode(CameraMovement.CameraMode.Aim);
        }
        else
        {
            _cmf.GetComponent<CameraMovement>().SetCameraMode(CameraMovement.CameraMode.Normal);
        }
    }

    protected virtual void Shoot()
    {
        
    }
}
