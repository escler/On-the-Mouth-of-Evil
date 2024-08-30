using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class HouseEnemy : Enemy
{
    public float speed;
    public LayerMask obstacles;
    public PathFinding pf; 

    private FiniteStateMachine _fsm;
    [SerializeField] private HouseEnemy_Idle idleState;
    [SerializeField] private HouseEnemy_Patrol patrolState;
    [SerializeField] private HouseEnemy_Chase chaseState;
    [SerializeField] private HouseEnemy_GoToLocation goToLocationState;
    
    private void Start()
    {
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
    }

    private void Awake()
    {
        OnAwake();
    }
}
