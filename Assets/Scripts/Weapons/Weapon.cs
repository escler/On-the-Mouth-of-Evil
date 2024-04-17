using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected float fireRate;
    [SerializeField] protected LayerMask layerMask;
    protected enum WeaponType
    {
        Ranged,
        Melee
    }

    protected virtual void OnDamage()
    {
        
    }
}
