using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoHandler : MonoBehaviour
{
    public static AmmoHandler Instance { get; private set; }
    private int _revolverBullets, _shotgunBullets;

    public int RevolverBullets => _revolverBullets;
    public int ShotgunBullets => _shotgunBullets;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }

        Instance = this;
    }

    public void AddBullet(GunType gunType, int amount)
    {
        switch (gunType)
        {
            case GunType.revolver: _revolverBullets += amount;
                break;
            case GunType.shotgun: _shotgunBullets += amount; break;
        }
        
        GetComponent<WeaponsHandler>().UpdateMaxBullet();
    }

    public void UpdateMaxAmount(GunType gunType, int amount)
    {
        switch (gunType)
        {
            case GunType.revolver: _revolverBullets = amount; break;
            case GunType.shotgun: _shotgunBullets = amount; break;
        }
        
        GetComponent<WeaponsHandler>().UpdateMaxBullet();
    }
}

public enum GunType
{
    revolver,
    shotgun
}
