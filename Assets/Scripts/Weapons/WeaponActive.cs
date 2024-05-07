using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActive : MonoBehaviour
{
    public List<RangedWeapon> weapons = new List<RangedWeapon>();
    public delegate void UpdateBulletUI();
    public event UpdateBulletUI OnUpdateBulletUI;
    [SerializeField] RangedWeapon _activeWeapon;
    private AnimPlayer _anim;

    private int _actualBullets;
    private void Awake()
    {
        _activeWeapon = weapons[0];
        _anim = GetComponentInParent<AnimPlayer>();
    }

    public int ActualBullet => _actualBullets;

    private void Update()
    {
        if(Input.GetButton("Weapon1")) ChangeWeapon(0);
        if(Input.GetButton("Weapon2"))ChangeWeapon(1);
        
        if(Input.GetButtonDown("Reload")) _activeWeapon.Reload();

    }

    private void ChangeWeapon(int value)
    {
        if (weapons.Count == 1 || value > weapons.Count|| weapons[value] == _activeWeapon) return;

        _activeWeapon.enabled = false;

        _activeWeapon = weapons[value];
        
        _activeWeapon.enabled = true;
        
        _anim.ChangeLayerHeight(value);
        Player.Instance.chest = _activeWeapon.toAim;
        RefreshData();
    }

    public void RefreshData()
    {
        _actualBullets = _activeWeapon.ActualBullet;
        OnUpdateBulletUI?.Invoke();
    }
}
