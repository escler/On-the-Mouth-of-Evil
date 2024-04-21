using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public abstract class RangedWeapon : Weapon
{
    private WeaponType _weaponType = WeaponType.Ranged;
    protected WeaponFeedback _weaponFeedback;
    private bool _aiming;
    private CinemachineFreeLook _cmf;
    protected Transform camera, targetAim;
    [SerializeField] private Crosshair _crosshair;
    protected float actualCd;

    private void Start()
    {
        camera = Camera.main.transform;
        targetAim = Player.Instance.targetAim;
        _cmf = FindObjectOfType<CinemachineFreeLook>();
        _weaponFeedback = GetComponent<WeaponFeedback>();
    }

    protected void OnUpdate()
    {
        _aiming = Input.GetMouseButton(1);
        if (actualCd > 0) actualCd -= Time.deltaTime;
        if (_aiming)
        {
            _crosshair.TurnOn();
            _cmf.GetComponent<CameraMovement>().SetCameraMode(CameraMovement.CameraMode.Aim);
            Aim();
            if (Input.GetMouseButtonDown(0) && actualCd <= 0)
            {
                Shoot();
                _weaponFeedback.FireParticle();
            }
        }
        else
        {
            _crosshair.TurnOff();
            _cmf.GetComponent<CameraMovement>().SetCameraMode(CameraMovement.CameraMode.Normal);
        }
    }

    protected abstract void Aim();

    protected abstract void Shoot();
}
