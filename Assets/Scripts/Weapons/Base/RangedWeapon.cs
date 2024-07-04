using Cinemachine;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class RangedWeapon : Weapon
{
    public int bulletsPerCharge;
    public int initialMaxAmmo;
    private int _chargerBullets, _maxBullets, _actualBullets;
    protected WeaponFeedback _weaponFeedback;
    private bool _aiming;
    private CinemachineFreeLook _cmf;
    protected Transform cameraPos, targetAim;
    [SerializeField] private Crosshair _crosshair;
    protected float actualCd;
    public float reloadTime;
    private float _actualReloadCd;
    private WeaponsHandler _weaponsHandler;
    private bool _shooting;
    [SerializeField] private GameObject[] psFire;
    private CinemachineImpulseSource _recoil;
    private AnimPlayer _anim;
    public float recoil;
    public int ChargerBullets => _chargerBullets;

    public int force;
    public bool Shooting => _shooting;
    public int MaxBullets
    {
        get { return _maxBullets; }
        set { _maxBullets = value; }
    }

    private void Start()
    {
        cameraPos = Camera.main.transform;
        targetAim = Player.Instance.targetAim;
        _cmf = FindObjectOfType<CinemachineFreeLook>();
        _chargerBullets = bulletsPerCharge;
        _weaponsHandler.RefreshData();
        _recoil = GetComponentInChildren<CinemachineImpulseSource>();
        _anim = Player.Instance.GetComponentInChildren<AnimPlayer>();
    }

    protected void OnUpdate()
    {
        _aiming = Input.GetMouseButton(1);
        _shooting = _anim.Shooting;
        if (actualCd > 0) actualCd -= Time.deltaTime;
        if (_actualReloadCd > 0) _actualReloadCd -= Time.deltaTime;
        if (_aiming)
        {
            _crosshair.OnAim();
            _cmf.GetComponent<CameraMovement>().SetCameraMode(CameraMovement.CameraMode.Aim);
            Aim();
            if (Input.GetMouseButtonDown(0) && !_shooting)
            {
                _shooting = true;

                _anim.Shooting = true;
            }
        }
        else
        {
            _crosshair.OffAim();
            _cmf.GetComponent<CameraMovement>().SetCameraMode(CameraMovement.CameraMode.Normal);
        }
    }
    
    private void OnDisable()
    {
        model.SetActive(false);
        _actualBullets = _chargerBullets;
    }

    private void OnEnable()
    {
        model.SetActive(true);
        var recoilDef = GetComponentInChildren<CinemachineImpulseSource>().m_ImpulseDefinition;
        recoilDef.m_AmplitudeGain = recoil;
        _weaponsHandler = GetComponent<WeaponsHandler>();
        _chargerBullets = _actualBullets;
        _weaponsHandler.RefreshData();
        _weaponFeedback = GetComponent<WeaponFeedback>();
        _weaponFeedback.SetFireParticle(psFire);
    }

    public void DoFeedbackShoot()
    {
        Shoot();
        _recoil.GenerateImpulse(Camera.main.transform.forward);
        _chargerBullets--;
        _weaponsHandler.RefreshData();
        _weaponFeedback.FireParticle();
    }

    
    protected abstract void Aim();

    protected abstract void Shoot();
}
