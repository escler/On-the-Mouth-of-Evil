using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; set; }
    public WeaponsHandler weaponHandler;
    public Movement movement;
    public InteractChecker interactChecker;
    public PlayerLifeHandler playerLifeHandler;
    public BookSkillTrigger bookSkill;
    public BossSkill bossSkill;
    public PlayerEnergyHandler playerEnergyHandler;
    public AnimPlayer playerAnim;
    public Transform chest, targetAim;
    public GameObject sphere;
    private bool _skillAdquired;
    public Transform bookPos;
    public bool cantUse; 

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
        movement.cantMove = true;
        interactChecker.enabled = false;
        if(_skillAdquired) bossSkill.enabled = false;
        bookSkill.enabled = false;
        cantUse = true;

    }

    public void PossesControls()
    {
        movement.cantMove = false;
        interactChecker.enabled = true;
        if(_skillAdquired) bossSkill.enabled = true;
        bookSkill.enabled = true;
        cantUse = false;
    }
    
    public void LevelUp()
    {
        Instance.playerAnim.SetSpeedValue(3f);
        movement.dashTime *= 2;
        var weapons = weaponHandler.weapons;
        foreach (var weapon in weapons)
        {
            weapon.damage *= 2;
            weapon.reloadTime /= 2;
        }
    }

    public void EnableSkill()
    {
        bossSkill.enabled = true;
    }
}
