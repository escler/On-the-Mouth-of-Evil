using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private GameObject _hitbox;

    public void SpawnHitBox()
    {
        Instantiate(_hitbox, transform.position, transform.rotation);
    }
}
