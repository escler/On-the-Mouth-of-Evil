using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected int _damage;
    protected float _cdForAttack;
    protected enum WeaponType
    {
        Ranged,
        Melee
    }

    protected virtual void OnDamage()
    {
        
    }
    
    
}
