using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FSM;
using UnityEngine;
using Random = UnityEngine.Random;

public class MorgueEnemy : Enemy
{
    public static MorgueEnemy Instance { get; private set; }

    public LayerMask obstacles;
    public PathFinding pf;
    public Room actualRoom, crossRoom;
    public bool crossUsed;
    private PlayerHandler _player;
    public float actualTime, timeToShowMe;
    private List<IInteractableEnemy> _objects;
    public bool compareRoom;
    private bool _canInteractWithObjectsEnv;
    public float actualTimeToLost;
    

    private FiniteStateMachine _fsm;
    [SerializeField] private MorgueEnemy_Idle idleState;
    [SerializeField] private MorgueEnemy_Patrol patrolState;
    [SerializeField] private MorgueLevel_ToLocation goToLocationState;

    public bool ritualDone;
    public Node nodeRitual;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        _objects = new List<IInteractableEnemy>();
        _player = PlayerHandler.Instance;
        pf = new PathFinding();
        _fsm = new FiniteStateMachine(idleState, StartCoroutine);
        
        //Idle
        _fsm.AddTransition(StateTransitions.ToPatrol, idleState, patrolState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, idleState, goToLocationState);
        
        //Patrol
        _fsm.AddTransition(StateTransitions.ToIdle, patrolState, idleState);
        _fsm.AddTransition(StateTransitions.ToPatrol, patrolState, patrolState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, patrolState, goToLocationState);
        
        //ToLocation
        _fsm.AddTransition(StateTransitions.ToIdle, goToLocationState, idleState);
        _fsm.AddTransition(StateTransitions.ToPatrol, goToLocationState, patrolState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, goToLocationState, goToLocationState);


        _fsm.Active = true;
        OnAwake();
        
    }


    private void Update()
    {
        CompareRooms();
        TimeToShow();
        
        if (actualTimeToLost > 0) actualTimeToLost -= Time.deltaTime;
        compareRoom = _player.actualRoom == actualRoom;
    }

    private void CompareRooms()
    {
        if (_player.actualRoom == null) return;
        if (_player.actualRoom != actualRoom)
        {
            if (_objects.Count() > 0)
            {
                foreach (var obj in _objects)
                {
                    obj.OnEndInteract();
                }
            }

            _objects.Clear();
            _canInteractWithObjectsEnv = true;
            return;
        }
        
        if (!_canInteractWithObjectsEnv) return;
        print("Room Igual");
        _canInteractWithObjectsEnv = false;
        var objectsInRoom = actualRoom.interactableEnemy.ToList();
        for (int i = 0; i < 6; i++)
        {
            if (objectsInRoom.Count() == 0) break;
            int randomIndex = Random.Range(0, objectsInRoom.Count);
            var actualObject = objectsInRoom[randomIndex];
            _objects.Add(actualObject.GetComponent<IInteractableEnemy>());
            objectsInRoom.Remove(actualObject);
        }

        foreach (var obj in _objects)
        {
            obj.OnStartInteract();
        }
    }

    private void TimeToShow()
    {
        if (!compareRoom)
        {
            actualTime = 0;
        }
        else actualTime += Time.deltaTime;
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
        //_enemyAnimator.ChangeStateAnimation("CrossUsed", true);
    }
}
