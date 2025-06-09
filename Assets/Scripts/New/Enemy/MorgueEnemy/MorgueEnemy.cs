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
    public bool crossUsed, inRitualNode;
    private PlayerHandler _player;
    public float actualTime, timeToShowMe;
    private List<IInteractableEnemy> _objects;
    public bool compareRoom, enemyVisible;
    private bool _canInteractWithObjectsEnv;
    public float actualTimeToLost;
    public bool _corroutineActivate;

    public GameObject absorbVFX, magnetVFX;
    

    private FiniteStateMachine _fsm;
    [SerializeField] private MorgueEnemy_Spawn spawnState;
    [SerializeField] private MorgueEnemy_Idle idleState;
    [SerializeField] private MorgueEnemy_Patrol patrolState;
    [SerializeField] private MorgueEnemy_Attacks attacksState;
    [SerializeField] private MorgueEnemy_ToLocation goToLocationState;
    [SerializeField] private MorgueEnemy_Ritual ritualState;
    [SerializeField] private MorgueEnemy_Voodoo voodooState;
    
    public bool ritualDone;
    public Node nodeRitual;

    public Material enemyMaterial;
    public float enemyVisibility;

    public bool canAttackPlayer;
    public bool appear;
    
    public bool voodooActivate;
    public Vector3 voodooPosition;

    public bool attackEnded;

    public Vector3 reference = Vector3.zero;
    public float rotationSmoothTime;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        enemyMaterial.SetFloat("_Power", 0);
        enemyVisibility = enemyMaterial.GetFloat("_Power");
        
        enemyMaterial.SetFloat("_intensity", 0.01f);

        _objects = new List<IInteractableEnemy>();
        _player = PlayerHandler.Instance;
        pf = new PathFinding();
        _fsm = new FiniteStateMachine(idleState, StartCoroutine);
        
        //Spawn
        _fsm.AddTransition(StateTransitions.ToAttacks, spawnState, attacksState);
        _fsm.AddTransition(StateTransitions.ToIdle, spawnState, idleState);
        _fsm.AddTransition(StateTransitions.ToVoodoo, spawnState, voodooState);
        
        //Idle
        _fsm.AddTransition(StateTransitions.ToPatrol, idleState, patrolState);
        _fsm.AddTransition(StateTransitions.ToAttacks, idleState, attacksState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, idleState, goToLocationState);
        _fsm.AddTransition(StateTransitions.ToRitual, idleState, ritualState);
        _fsm.AddTransition(StateTransitions.ToSpawn, idleState, spawnState);
        _fsm.AddTransition(StateTransitions.ToVoodoo, idleState, voodooState);
        
        //Patrol
        _fsm.AddTransition(StateTransitions.ToIdle, patrolState, idleState);
        _fsm.AddTransition(StateTransitions.ToAttacks, patrolState, attacksState);
        _fsm.AddTransition(StateTransitions.ToPatrol, patrolState, patrolState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, patrolState, goToLocationState);
        _fsm.AddTransition(StateTransitions.ToRitual, patrolState, ritualState);
        _fsm.AddTransition(StateTransitions.ToVoodoo, patrolState, voodooState);
        
        //Attack
        _fsm.AddTransition(StateTransitions.ToIdle, attacksState, idleState);
        _fsm.AddTransition(StateTransitions.ToPatrol, attacksState, patrolState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, attacksState, goToLocationState);
        _fsm.AddTransition(StateTransitions.ToRitual, attacksState, ritualState);
        _fsm.AddTransition(StateTransitions.ToAttacks, attacksState, attacksState);
        _fsm.AddTransition(StateTransitions.ToVoodoo, attacksState, voodooState);
        
        //ToLocation
        _fsm.AddTransition(StateTransitions.ToIdle, goToLocationState, idleState);
        _fsm.AddTransition(StateTransitions.ToPatrol, goToLocationState, patrolState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, goToLocationState, goToLocationState);
        _fsm.AddTransition(StateTransitions.ToRitual, goToLocationState, ritualState);

        //Voodoo
        _fsm.AddTransition(StateTransitions.ToPatrol, voodooState, patrolState);
        _fsm.AddTransition(StateTransitions.ToRitual, voodooState, ritualState);

        _fsm.Active = true;
        OnAwake();
        
    }


    private void Update()
    {
        CompareRooms();
        TimeToShow();
        
        if (actualTimeToLost > 0) actualTimeToLost -= Time.deltaTime;
        compareRoom = _player.actualRoom == actualRoom;
        canAttackPlayer = enemyVisible && actualTimeToLost > 0;
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
    
    public void HideEnemy()
    {
        if (_corroutineActivate) return;
        
        StartCoroutine(HideEnemyLerp());
        //_enemyAnimator.ChangeStateAnimation("Spawn", false);
        appear = false;
    }
    
    IEnumerator HideEnemyLerp()
    {
        _corroutineActivate = true;
        while (enemyVisibility > 0)
        {
            enemyVisibility -= .3f;
            enemyMaterial.SetFloat("_Power", enemyVisibility);
            yield return new WaitForSeconds(0.1f);
        }
        enemyVisible = false;
        _corroutineActivate = false;
    }
}
