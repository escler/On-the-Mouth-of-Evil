using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public abstract class RangedWeapon : Weapon
{
    public int maxBullets;
    private int _actualBullet;
    protected WeaponFeedback _weaponFeedback;
    private bool _aiming;
    private CinemachineFreeLook _cmf;
    protected Transform cameraPos, targetAim;
    [SerializeField] private Crosshair _crosshair;
    protected float actualCd;
    public float reloadTime;
    private float _actualReloadCd;
    private WeaponActive _weaponActive;

    public int ActualBullet => _actualBullet;
    private void Start()
    {
        cameraPos = Camera.main.transform;
        targetAim = Player.Instance.targetAim;
        _cmf = FindObjectOfType<CinemachineFreeLook>();
        _weaponFeedback = GetComponent<WeaponFeedback>();
    }

    protected void OnUpdate()
    {
        _aiming = Input.GetMouseButton(1);
        if (actualCd > 0) actualCd -= Time.deltaTime;
        if (_actualReloadCd > 0) _actualReloadCd -= Time.deltaTime;
        if (_aiming)
        {
            _crosshair.TurnOn();
            _cmf.GetComponent<CameraMovement>().SetCameraMode(CameraMovement.CameraMode.Aim);
            Aim();
            if (Input.GetMouseButtonDown(0) && actualCd <= 0 && _actualBullet > 0)
            {
                Shoot();
                _actualBullet--;
                _weaponActive.RefreshData();
                _weaponFeedback.FireParticle();
            }
        }
        else
        {
            _crosshair.TurnOff();
            _cmf.GetComponent<CameraMovement>().SetCameraMode(CameraMovement.CameraMode.Normal);
        }
    }

    public void Reload()
    {
        if(_actualReloadCd > 0) return;
        _actualBullet = maxBullets;
        _actualReloadCd = reloadTime;
        _weaponActive.RefreshData();
    }
    
    private void OnDisable()
    {
        model.SetActive(false);
    }

    private void OnEnable()
    {
        model.SetActive(true);
        _actualBullet = maxBullets;
        _weaponActive = GetComponent<WeaponActive>();
        _weaponActive.RefreshData();
        
    }
    
    protected abstract void Aim();

    protected abstract void Shoot();
}
