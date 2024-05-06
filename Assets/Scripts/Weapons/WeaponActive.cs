using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActive : MonoBehaviour
{
    public List<Weapon> weapons = new List<Weapon>();
    public delegate void UpdateBulletUI();
    public event UpdateBulletUI OnUpdateBulletUI;
    private Weapon _activeWeapon;
    private AnimPlayer _anim;
    private void Awake()
    {
        //Player.Instance.activeWeapon = weapons[0];
        _activeWeapon = weapons[0];
        _anim = GetComponentInParent<AnimPlayer>();
    }

    public int ActualBullet => _activeWeapon.GetComponent<RangedWeapon>().ActualBullet;

    private void Update()
    {
        if(Input.GetButton("Weapon1")) ChangeWeapon(0);
        if(Input.GetButton("Weapon2"))ChangeWeapon(1);
        
        if(Input.GetButton("Reload")) _activeWeapon.GetComponent<RangedWeapon>().Reload();
    }

    private void ChangeWeapon(int value)
    {
        if (weapons.Count == 1 || value > weapons.Count|| weapons[value] == _activeWeapon) return;

        _activeWeapon.enabled = false;

        _activeWeapon = weapons[value];
        
        _activeWeapon.enabled = true;
        
        _anim.ChangeLayerHeight(value);
        Player.Instance.chest = _activeWeapon.toAim;
        
        
        Player.Instance.activeWeapon = _activeWeapon;
        
    }
}
