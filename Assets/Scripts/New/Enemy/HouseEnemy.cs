using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FSM;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class HouseEnemy : Enemy
{
    public static HouseEnemy Instance;
    public float speed;
    public LayerMask obstacles;
    public PathFinding pf;
    public Room actualRoom, crossRoom;
    public bool crossUsed; 
    private PlayerHandler _player;
    public float actualTime, timeToShowMe;
    private List<IInteractableEnemy> objects;
    private bool canInteract;
    public SkinnedMeshRenderer mesh;
    PlayParticles Fire;
    public bool appear;
    [SerializeField] public GameObject lavaPrefab;
    private Vector3 targetScale;

    private FiniteStateMachine _fsm;
    [SerializeField] private HouseEnemy_Spawn spawnState;
    [SerializeField] private HouseEnemy_Idle idleState;
    [SerializeField] private HouseEnemy_Patrol patrolState;
    [SerializeField] private HouseEnemy_Attacks attacksState;
    [SerializeField] private HouseEnemy_GoToLocation goToLocationState;
    [SerializeField] private HouseEnemy_Ritual ritualState;
    [SerializeField] private HouseEnemy_GrabHead grabHeadState;
    private bool hasPlayedFire;
    public bool ritualDone;
    public Node nodeRitual;

    public Material enemyMaterial;
    public float enemyVisibility;
    private bool _corroutineActivate;

    public bool onRitual;

    private CorduraHandler _corduraHandler;


    private HouseEnemyView _enemyAnimator;

    public HouseEnemyView EnemyAnimator => _enemyAnimator;

    public bool grabHead;
    public Transform headPos;

    public bool attackEnded;

    public bool enemyVisible, canAttackPlayer, compareRoom;
    public float actualTimeToLost;
    public bool activateExorcism;

    public Transform obstaclePosRight, obstaclePosLeft;
    public float distanceObstacleRay;
    public float intensityObstacleAvoidance;
    public Vector3 reference = Vector3.zero;
    public CapsuleCollider capsule;
    public float rotationSmoothTime;

    public int playerGrabbedCount;
    public ParticleSystem[] smokePS;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        capsule.GetComponent<CapsuleCollider>();
        _corduraHandler = CorduraHandler.Instance;
        _enemyAnimator = GetComponentInChildren<HouseEnemyView>();
        enemyMaterial.SetFloat("_Power", 0);
        enemyVisibility = enemyMaterial.GetFloat("_Power");


        objects = new List<IInteractableEnemy>();
        _player = PlayerHandler.Instance;
        pf = new PathFinding();
        _fsm = new FiniteStateMachine(idleState, StartCoroutine);

        //Spawn
        _fsm.AddTransition(StateTransitions.ToAttacks, spawnState, attacksState);
        
        //Idle
        _fsm.AddTransition(StateTransitions.ToPatrol, idleState, patrolState);
        _fsm.AddTransition(StateTransitions.ToAttacks, idleState, attacksState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, idleState, goToLocationState);
        _fsm.AddTransition(StateTransitions.ToRitual, idleState, ritualState);
        _fsm.AddTransition(StateTransitions.ToSpawn, idleState, spawnState);
        
        //Patrol
        _fsm.AddTransition(StateTransitions.ToIdle, patrolState, idleState);
        _fsm.AddTransition(StateTransitions.ToAttacks, patrolState, attacksState);
        _fsm.AddTransition(StateTransitions.ToPatrol, patrolState, patrolState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, patrolState, goToLocationState);
        _fsm.AddTransition(StateTransitions.ToRitual, patrolState, ritualState);
        
        //Attack
        _fsm.AddTransition(StateTransitions.ToIdle, attacksState, idleState);
        _fsm.AddTransition(StateTransitions.ToPatrol, attacksState, patrolState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, attacksState, goToLocationState);
        _fsm.AddTransition(StateTransitions.ToRitual, attacksState, ritualState);
        _fsm.AddTransition(StateTransitions.ToAttacks, attacksState, attacksState);

        
        //GoToLocation
        _fsm.AddTransition(StateTransitions.ToIdle, goToLocationState, spawnState);
        _fsm.AddTransition(StateTransitions.ToIdle, goToLocationState, idleState);
        _fsm.AddTransition(StateTransitions.ToPatrol, goToLocationState, patrolState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, goToLocationState, goToLocationState);
        _fsm.AddTransition(StateTransitions.ToRitual, goToLocationState, ritualState);

        //GoToGrabHead
        _fsm.AddTransition(StateTransitions.ToIdle, grabHeadState, idleState);
        
        _fsm.Active = true;
        OnAwake();
        appear = false;
    }

    private void Update()
    {
        if (onRitual) return;
        CompareRooms();
        TimeToShow();

        if (actualTimeToLost > 0) actualTimeToLost -= Time.deltaTime;
        compareRoom = _player.actualRoom == actualRoom;
        canAttackPlayer = enemyVisible && actualTimeToLost > 0;
    }

    private void TimeToShow()
    {
        if (!compareRoom)
        {
            actualTime = 0;
        }
        else actualTime += Time.deltaTime;
    }

    public void HideEnemy()
    {
        if (_corroutineActivate) return;
        
        StartCoroutine(HideEnemyLerp());
        _enemyAnimator.ChangeStateAnimation("Spawn", false);
        appear = false;
    }

    public void ShowEnemyRitual()
    {
        if (!onRitual) Ritual();
        StopCoroutine(HideEnemyLerp());
    }

    void Ritual()
    {
        if (TarotCardPuzzle.Instance.BadPathTaked) StartCoroutine(ShowEnemyOnBadRitual());
        else StartCoroutine(ShowEnemyOnGoodRitual());
        onRitual = true;

    }

    IEnumerator ShowEnemyOnBadRitual()
    {
        yield return null;
    }
    
    IEnumerator ShowEnemyOnGoodRitual()
    {
        while (enemyVisibility < 8)
        {
            enemyVisibility += .3f;
            if (enemyVisibility >= 7.8f)
            {
                _enemyAnimator.animator.applyRootMotion = true;
                _enemyAnimator.ChangeStateAnimation("Exorcism", true);
            }
            enemyMaterial.SetFloat("_Power", enemyVisibility);

            yield return new WaitForSeconds(0.1f);
        }


        if(!_enemyAnimator.animator.hasRootMotion)_enemyAnimator.animator.applyRootMotion = true;
        activateExorcism = true;

        RitualManager.Instance.ritualFloor.SetActive(false);

        RitualManager.Instance.ActivateCraterFloor();
        yield return new WaitUntil(
            () => _enemyAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName("RitualExorcism"));
        
        yield return new WaitForSeconds(4.8f);
        
        while (enemyVisibility > 0)
        {
            enemyVisibility -= .3f;
            enemyMaterial.SetFloat("_Power", enemyVisibility);
            
            yield return new WaitForSeconds(0.1f);
        }

        RitualManager.Instance.CloseCrater();
        
        TarotCardPuzzle.Instance.PathTaked();
        GameManagerNew.Instance.LoadSceneWithDelay("Hub",3);
        RitualManager.Instance.RitualFinish();
        gameObject.SetActive(false);
    }
    
    IEnumerator HideEnemyLerp()
    {
        _corroutineActivate = true;
        foreach (var ps in smokePS)
        {
            ps.Stop();
        }
        while (enemyVisibility > 0)
        {
            enemyVisibility -= .3f;
            enemyMaterial.SetFloat("_Power", enemyVisibility);
            yield return new WaitForSeconds(0.1f);
        }
        enemyVisible = false;
        _corroutineActivate = false;
    }

    private void CompareRooms()
    {
        if (_player.actualRoom == null) return;
        if (_player.actualRoom != actualRoom)
        {
            if (objects.Count() > 0)
            {
                foreach (var obj in objects)
                {
                    obj.OnEndInteract();
                }
            }
            objects.Clear();
            canInteract = true;
            return;
        }

        if (!canInteract) return;
        canInteract = false;
        var objectsInRoom = actualRoom.interactableEnemy.ToList();
        for (int i = 0; i < 6; i++)
        {
            if (objectsInRoom.Count() == 0) break;
            int randomIndex = Random.Range(0, objectsInRoom.Count);
            var actualObject = objectsInRoom[randomIndex];
            objects.Add(actualObject.GetComponent<IInteractableEnemy>());
            objectsInRoom.Remove(actualObject);
        }

        foreach (var obj in objects)
        {
            obj.OnStartInteract();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 16) return;

        actualRoom = other.GetComponent<Room>();
    }

    public void CheckRoom()
    {
        if (actualRoom != PlayerHandler.Instance.actualRoom) return;
        crossUsed = true;
        _enemyAnimator.ChangeStateAnimation("CrossUsed", true);
    }

    public void RitualReady(Node node)
    {
        nodeRitual = node;
        ritualDone = true;
    }

    public Vector3 MoveSmooth(Vector3 target)
    {
        var dir = (target - transform.position).normalized;

        return dir;
    }

}
