using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponType weaponType;
    [SerializeField] protected int damage;
    [SerializeField] protected float fireRate;
    [SerializeField] protected LayerMask layerMask;

    protected virtual void OnDamage()
    {
        
    }
}
