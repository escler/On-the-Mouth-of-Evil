using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; set; }
    public Weapon activeWeapon;
    public Movement movement;
    public InteractChecker interactChecker;
    public Transform chest, targetAim;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void DipposeControls()
    {
        movement.enabled = false;
        interactChecker.enabled = false;
    }

    public void PossesControls()
    {
        movement.enabled = true;
        interactChecker.enabled = true;
    }

}
