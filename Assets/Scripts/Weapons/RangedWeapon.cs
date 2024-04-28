using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public abstract class RangedWeapon : Weapon
{
    public int maxBullets;
    public delegate void UpdateBulletUI();
    public event UpdateBulletUI OnUpdateBulletUI;
    private int _actualBullet;
    protected WeaponFeedback _weaponFeedback;
    private bool _aiming;
    private CinemachineFreeLook _cmf;
    protected Transform cameraPos, targetAim;
    [SerializeField] private Crosshair _crosshair;
    protected float actualCd;
    public float reloadTime;
    private float _actualReloadCd;

    public int ActualBullet => _actualBullet;
    private void Start()
    {
        cameraPos = Camera.main.transform;
        targetAim = Player.Instance.targetAim;
        _cmf = FindObjectOfType<CinemachineFreeLook>();
        _weaponFeedback = GetComponent<WeaponFeedback>();
        _actualBullet = maxBullets;
        OnUpdateBulletUI?.Invoke();
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
                OnUpdateBulletUI?.Invoke();
                _weaponFeedback.FireParticle();
            }
        }
        else
        {
            _crosshair.TurnOff();
            _cmf.GetComponent<CameraMovement>().SetCameraMode(CameraMovement.CameraMode.Normal);
        }

        if (Input.GetButtonDown("Reload") && _actualReloadCd <= 0)
        {
            Reload();
        }
    }

    public void Reload()
    {
        _actualBullet = maxBullets;
        _actualReloadCd = reloadTime;
        OnUpdateBulletUI.Invoke();
    }
    protected abstract void Aim();

    protected abstract void Shoot();
}
