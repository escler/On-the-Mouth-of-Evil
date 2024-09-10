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
    public Room actualRoom;
    private PlayerHandler _player;
    public float actualTime, timeToShowMe;
    private List<IInteractableEnemy> objects;
    private bool canInteract;
    public MeshRenderer mesh;
    public Transform Pivot;
    public GameObject PS;
    PlayParticles Fire;
    public bool appear;
    private FiniteStateMachine _fsm;
    [SerializeField] private HouseEnemy_Idle idleState;
    [SerializeField] private HouseEnemy_Patrol patrolState;
    [SerializeField] private HouseEnemy_Chase chaseState;
    [SerializeField] private HouseEnemy_GoToLocation goToLocationState;
    private bool hasPlayedFire;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        objects = new List<IInteractableEnemy>();
        _player = PlayerHandler.Instance;
        pf = new PathFinding();
        _fsm = new FiniteStateMachine(idleState, StartCoroutine);

        //Idle
        _fsm.AddTransition(StateTransitions.ToPatrol, idleState, patrolState);
        _fsm.AddTransition(StateTransitions.ToChase, idleState, chaseState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, idleState, goToLocationState);
        
        //Patrol
        _fsm.AddTransition(StateTransitions.ToIdle, patrolState, idleState);
        _fsm.AddTransition(StateTransitions.ToChase, patrolState, chaseState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, patrolState, goToLocationState);

        
        //Chase
        _fsm.AddTransition(StateTransitions.ToIdle, chaseState, idleState);
        _fsm.AddTransition(StateTransitions.ToPatrol, chaseState, patrolState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, chaseState, goToLocationState);
        
        //GoToLocation
        _fsm.AddTransition(StateTransitions.ToIdle, goToLocationState, idleState);
        
        _fsm.Active = true;
        OnAwake();
        appear = false;
    }

    private void Update()
    {
        CompareRooms();
        ShowEnemy();

    }

    private void ShowEnemy()
    {
        if (_player.actualRoom == null) return;
        if (_player.actualRoom != actualRoom)
        {
            actualTime = 0;
            if (mesh.enabled)
            {
                mesh.enabled = false;
               
            }
            hasPlayedFire = false;
            return;
        }

        actualTime += Time.deltaTime;

        if (actualTime > timeToShowMe)
        {
            mesh.enabled = true;
            appear = true;
            if (appear && !hasPlayedFire)
            {
                PS.SetActive(true);

            }
            appear = false;
        }
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
}
