using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FSM;
using UnityEngine;
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
    private FiniteStateMachine _fsm;
    [SerializeField] private HouseEnemy_Idle idleState;
    [SerializeField] private HouseEnemy_Patrol patrolState;
    [SerializeField] private HouseEnemy_Chase chaseState;
    [SerializeField] private HouseEnemy_GoToLocation goToLocationState;
    [SerializeField] private HouseEnemy_Ritual goToRitual;
    private bool hasPlayedFire;
    public bool ritualDone;
    public Node nodeRitual;

    public Material enemyMaterial;
    private float _enemyVisibility;
    private bool _corroutineActivate;

    public bool onRitual;

    private CorduraHandler _corduraHandler;


    private HouseEnemyView _enemyAnimator;

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
        enemyMaterial.SetFloat("_Power", 0);
        _enemyVisibility = enemyMaterial.GetFloat("_Power");
        
        objects = new List<IInteractableEnemy>();
        _player = PlayerHandler.Instance;
        pf = new PathFinding();
        _fsm = new FiniteStateMachine(idleState, StartCoroutine);

        //Idle
        _fsm.AddTransition(StateTransitions.ToPatrol, idleState, patrolState);
        _fsm.AddTransition(StateTransitions.ToChase, idleState, chaseState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, idleState, goToLocationState);
        _fsm.AddTransition(StateTransitions.ToRitual, idleState, goToRitual);
        
        //Patrol
        _fsm.AddTransition(StateTransitions.ToIdle, patrolState, idleState);
        _fsm.AddTransition(StateTransitions.ToChase, patrolState, chaseState);
        _fsm.AddTransition(StateTransitions.ToPatrol, patrolState, patrolState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, patrolState, goToLocationState);
        _fsm.AddTransition(StateTransitions.ToRitual, patrolState, goToRitual);


        
        //Chase
        _fsm.AddTransition(StateTransitions.ToIdle, chaseState, idleState);
        _fsm.AddTransition(StateTransitions.ToPatrol, chaseState, patrolState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, chaseState, goToLocationState);
        _fsm.AddTransition(StateTransitions.ToRitual, chaseState, goToRitual);

        
        //GoToLocation
        _fsm.AddTransition(StateTransitions.ToIdle, goToLocationState, idleState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, goToLocationState, goToLocationState);
        _fsm.AddTransition(StateTransitions.ToRitual, goToLocationState, goToRitual);

        
        _fsm.Active = true;
        OnAwake();
        appear = false;
    }

    private void Update()
    {
        if (onRitual) return;
        CompareRooms();
        ShowEnemy();

    }

    private void ShowEnemy()
    {
        if (_player.actualRoom == null) return;
        if (_player.actualRoom != actualRoom)
        {
            actualTime = 0;
            _corduraHandler.CorduraOn = false;
            //if (!_corroutineActivate && _enemyVisibility < 10)
           // {
           //     StartCoroutine(HideEnemy());
           // }
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
            }
            
            //if (!_corroutineActivate && _enemyVisibility > 0)
            //    StartCoroutine(ShowEnemyLerp());
        }
    }

    public void ShowEnemyRitual()
    {
        onRitual = true;
        StopCoroutine(ShowEnemyLerp());
        StopCoroutine(HideEnemy());
        StartCoroutine(ShowEnemyOnRitual());
    }
    
    IEnumerator ShowEnemyLerp()
    {
        _corroutineActivate = true;
        while (_enemyVisibility > 0)
        {
            _enemyVisibility -= .5f;
            enemyMaterial.SetFloat("_Power", _enemyVisibility);
            yield return new WaitForSeconds(0.1f);
        }
        _corduraHandler.CorduraOn = true;
        CorduraHandler.Instance.StartCordura();
        _corroutineActivate = false;
    }

    IEnumerator ShowEnemyOnRitual()
    {
        while (_enemyVisibility > 0)
        {
            _enemyVisibility -= .5f;
            enemyMaterial.SetFloat("_Power", _enemyVisibility);
            yield return new WaitForSeconds(0.1f);
        }
        
        yield return new WaitForSeconds(5f);
        
        while (_enemyVisibility < 10)
        {
            _enemyVisibility += .5f;
            enemyMaterial.SetFloat("_Power", _enemyVisibility);
            yield return new WaitForSeconds(0.1f);
        }
        
        gameObject.SetActive(false);
    }



    IEnumerator HideEnemy()
    {
        _corroutineActivate = true;
        while (_enemyVisibility < 10)
        {
            _enemyVisibility += .5f;
            enemyMaterial.SetFloat("_Power", _enemyVisibility);
            yield return new WaitForSeconds(0.1f);
        }

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
