using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; set; }
    public Weapon activeWeapon;
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

    public bool SkillAdquired
    {
        get => _skillAdquired;
        set => _skillAdquired = value;
    }
    
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
        bossSkill.enabled = false;
        bookSkill.enabled = false;

    }

    public void PossesControls()
    {
        movement.cantMove = false;
        interactChecker.enabled = true;
        bossSkill.enabled = true;
        bookSkill.enabled = true;
    }
}
