using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsHandler : MonoBehaviour
{
    public static WeaponsHandler Instance { get; set; }
    
    public List<RangedWeapon> weapons = new List<RangedWeapon>();
    public delegate void UpdateBulletUI();
    public event UpdateBulletUI OnUpdateBulletUI;
    [SerializeField] RangedWeapon _activeWeapon;
    private AnimPlayer _anim;
    public AudioSource weaponAudioSource;

    private int _actualBullets, _maxBullets;

    private void Start()
    {
        _anim = Player.Instance.GetComponentInChildren<AnimPlayer>();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        _activeWeapon = weapons[0];

    }

    public int ActualBullet => _actualBullets;
    public int MaxBullets => _maxBullets;

    private void Update()
    {
        if(Input.GetButton("Weapon1")) ChangeWeapon(0);
        if(Input.GetButton("Weapon2")) ChangeWeapon(1);
        
    }

    private void ChangeWeapon(int value)
    {
        if (weapons.Count == 1 || value > weapons.Count|| weapons[value] == _activeWeapon || _activeWeapon.Shooting) return;

        _activeWeapon.enabled = false;

        _activeWeapon = weapons[value];
        
        _activeWeapon.enabled = true;
        
        _anim.ChangeLayerHeight(value);
        Player.Instance.chest = _activeWeapon.toAim;
        RefreshData();
    }

    public void RefreshData()
    {
        if (_activeWeapon == null) return;
        _actualBullets = _activeWeapon.ChargerBullets;
        _maxBullets = _activeWeapon.MaxBullets;
        OnUpdateBulletUI?.Invoke();
    }

    public void AddWeapon(RangedWeapon rangedWeapon)
    {
        if (weapons.Contains(rangedWeapon)) return;

        weapons.Add(rangedWeapon);
    }

    public void FeedbackShoot()
    {
        _activeWeapon.DoFeedbackShoot();
    }
}
