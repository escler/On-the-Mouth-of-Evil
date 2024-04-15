using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; set; }
    [SerializeField] private Weapon _activeWeapon;
    public Transform chest, targetAim;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
    }

    public void ChangeWeapon(Weapon newWeapon)
    {
        _activeWeapon = newWeapon;
    }
}
