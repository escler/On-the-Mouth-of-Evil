using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponType weaponType;
    [SerializeField] public int damage;
    [SerializeField] protected float fireRate;
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected GameObject model;
    public Transform toAim;

    protected virtual void OnDamage()
    {
        
    }
}
