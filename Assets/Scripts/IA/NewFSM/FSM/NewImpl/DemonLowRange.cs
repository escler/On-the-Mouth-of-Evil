using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using Unity.VisualScripting;

public class DemonLowRange : MonoBehaviour
{
    public Transform target;

    [SerializeField] float distanceToAttack = 3;
    [SerializeField] float distanceToPersuit = 15;
    private float _cdForAttack;

    public float CdForAttack
    {
        get { return _cdForAttack; }
        set { _cdForAttack = value; }
    }
    private bool banished;

    FiniteStateMachine fsm;

    [SerializeField] LRDemonIdleState idleState;
    [SerializeField] LRDemonChaseState chaseState;
    [SerializeField] LRDAttackState attackState;
    [SerializeField] LRDDemonMoveAroundState moveAroundState;
    [SerializeField] LRDemonBanishState banishState;

    private void Start()
    {
        fsm = new FiniteStateMachine(idleState, StartCoroutine);

        //Idle
        fsm.AddTransition(StateTransitions.ToChase, idleState, chaseState);
        fsm.AddTransition(StateTransitions.ToAttack, idleState, attackState);
        fsm.AddTransition(StateTransitions.ToBanish, idleState, banishState);
        
        //Chase
        fsm.AddTransition(StateTransitions.ToAttack, chaseState, attackState);
        fsm.AddTransition(StateTransitions.ToBanish, chaseState, banishState);
        fsm.AddTransition(StateTransitions.ToMoveAround, chaseState, moveAroundState);
        
        //Attack
        fsm.AddTransition(StateTransitions.ToIdle, attackState, idleState);
        fsm.AddTransition(StateTransitions.ToBanish, attackState, banishState);

        //Move Around
        fsm.AddTransition(StateTransitions.ToChase, moveAroundState, chaseState);
        fsm.AddTransition(StateTransitions.ToBanish, moveAroundState, banishState);

        fsm.Active = true;
    }

    private void Awake()
    {
        target = Player.Instance.transform;
    }

    private void Update()
    {
        if (_cdForAttack > 0)
        {
            _cdForAttack -= Time.deltaTime;
        }
    }

    public bool IsPersuitDistance()
    {
        return Vector3.Distance(target.position, transform.position) <= distanceToPersuit;
    }

    public bool IsAttackDistance()
    {
        return Vector3.Distance(target.position, transform.position) <= distanceToAttack;
    }

    public bool CanAttack()
    {
        return _cdForAttack < 0;
    }

    public bool EnemyBanished()
    {
        return banished;
    }
}
