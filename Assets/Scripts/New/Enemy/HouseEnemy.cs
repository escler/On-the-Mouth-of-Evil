using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FSM;
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
    public Material lavaMaterial;
    private float _enemyVisibility;
    private bool _corroutineActivate;

    public bool onRitual;

    private CorduraHandler _corduraHandler;


    private HouseEnemyView _enemyAnimator;

    public HouseEnemyView EnemyAnimator => _enemyAnimator;

    public bool grabHead;
    public Transform headPos;

    public bool attackEnded;

    public ParticleSystem trailFire;
    public ParticleSystem closeSmokeParticles;
    public ParticleSystem trailSmokeParticles;



    public bool enemyVisible, canAttackPlayer, compareRoom;
    public float actualTimeToLost;
    public bool activateExorcism;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _corduraHandler = CorduraHandler.Instance;
        _enemyAnimator = GetComponentInChildren<HouseEnemyView>();
        enemyMaterial.SetFloat("_Power", 10);
        lavaMaterial.SetFloat("_Power", 10);
        closeSmokeParticles.Stop();
        trailSmokeParticles.Stop();
        _enemyVisibility = enemyMaterial.GetFloat("_Power");


        objects = new List<IInteractableEnemy>();
        _player = PlayerHandler.Instance;
        pf = new PathFinding();
        _fsm = new FiniteStateMachine(idleState, StartCoroutine);

        //Idle
        _fsm.AddTransition(StateTransitions.ToPatrol, idleState, patrolState);
        _fsm.AddTransition(StateTransitions.ToAttacks, idleState, attacksState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, idleState, goToLocationState);
        _fsm.AddTransition(StateTransitions.ToRitual, idleState, ritualState);
        
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
        _fsm.AddTransition(StateTransitions.ToIdle, goToLocationState, idleState);
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
        ShowEnemy();

        if (actualTimeToLost > 0) actualTimeToLost -= Time.deltaTime;
        compareRoom = _player.actualRoom == actualRoom;
        canAttackPlayer = enemyVisible && compareRoom;

    }

    private void ShowEnemy()
    {
        if (_player.actualRoom == null) return;
        if (actualTimeToLost > 0 && enemyVisible) return;
        if (_player.actualRoom != actualRoom)
        {
            actualTime = 0;
            if (!_corroutineActivate && _enemyVisibility < 10)
            {
                StartCoroutine(HideEnemy());
            }
            appear = false;
            hasPlayedFire = false;
            _enemyAnimator.ChangeStateAnimation("Spawn", false);
            return;
        }

        actualTime += Time.deltaTime;

        if (actualTime > timeToShowMe)
        {
            if (!appear)
            {
                _enemyAnimator.ChangeStateAnimation("Spawn", true);
                appear = true;
                //lavaPrefab.SetActive(true);
                //lavaPrefab.transform.localScale = Vector3.Lerp(Vector3.zero,targetScale, 2f);

            }

            if (!_corroutineActivate && _enemyVisibility > 0) StartCoroutine(ShowEnemyLerp());
        }
    }

    public void ShowEnemyRitual()
    {
        if(!onRitual)StartCoroutine(ShowEnemyOnRitual());
        onRitual = true;
        StopCoroutine(ShowEnemyLerp());
        StopCoroutine(HideEnemy());
    }
    
    IEnumerator ShowEnemyLerp()
    {
        _corroutineActivate = true;
        while (_enemyVisibility > 0)
        {
            _enemyVisibility -= .5f;
            enemyMaterial.SetFloat("_Power", _enemyVisibility);
            lavaMaterial.SetFloat("_Power", _enemyVisibility);
            yield return new WaitForSeconds(0.1f);
        }

        enemyVisible = true;
        closeSmokeParticles.Play();
        trailSmokeParticles.Play();
        trailFire.Play();
        _corroutineActivate = false;
    }

    IEnumerator ShowEnemyOnRitual()
    {
        while (_enemyVisibility > 0)
        {
            if (_enemyVisibility >= 0.2f)
            {
                _enemyAnimator.animator.applyRootMotion = true;
                _enemyAnimator.ChangeStateAnimation("Exorcism", true);
            }
            _enemyVisibility -= .5f;
            enemyMaterial.SetFloat("_Power", _enemyVisibility);
            lavaMaterial.SetFloat("_Power", _enemyVisibility);

            yield return new WaitForSeconds(0.1f);
        }

        activateExorcism = true;

        RitualManager.Instance.ritualFloor.SetActive(false);

        RitualManager.Instance.ActivateCraterFloor();
        yield return new WaitUntil(
            () => _enemyAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName("RitualExorcism"));
        
        yield return new WaitForSeconds(4.8f);
        
        while (_enemyVisibility < 10)
        {
            _enemyVisibility += .5f;
            enemyMaterial.SetFloat("_Power", _enemyVisibility);
            lavaMaterial.SetFloat("_Power", _enemyVisibility);

            yield return new WaitForSeconds(0.1f);
        }
        
        GameManagerNew.Instance.LoadSceneWithDelay("Hub",3);
        RitualManager.Instance.RitualFinish();
        gameObject.SetActive(false);
    }



    IEnumerator HideEnemy()
    {
        _corroutineActivate = true;
        while (_enemyVisibility < 10)
        {
            _enemyVisibility += .5f;
            enemyMaterial.SetFloat("_Power", _enemyVisibility);
            lavaMaterial.SetFloat("_Power", _enemyVisibility);
            yield return new WaitForSeconds(0.1f);
        }
        closeSmokeParticles.Stop();
        trailSmokeParticles.Stop();
        trailFire.Stop();
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

}
