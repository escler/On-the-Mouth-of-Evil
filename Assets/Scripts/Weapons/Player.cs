using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; set; }
    public Weapon activeWeapon;
    public Movement movement;
    public InteractChecker interactChecker;
    public PlayerLifeHandler playerLifeHandler;
    public PlayerEnergyHandler playerEnergyHandler;
    public Transform chest, targetAim;
    public GameObject sphere;
    
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
    }

    public void PossesControls()
    {
        movement.cantMove = false;
        interactChecker.enabled = true;
    }
}
