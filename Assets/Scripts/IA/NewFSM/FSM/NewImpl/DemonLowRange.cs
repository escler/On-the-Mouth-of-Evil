using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using Unity.VisualScripting;

public class DemonLowRange : MonoBehaviour, IBanishable
{
    public Transform target;

    [SerializeField] float distanceToAttack = 3;
    [SerializeField] float distanceToPersuit = 15;
    private float _cdForAttack;
    [SerializeField] private LayerMask _layer;
    private bool _ray;
    [SerializeField] private GameObject hitboxAttack;

    public float cdForAttack;
    private LRDemonAnim _animator;
    public LRDemonAnim animator => _animator;

    #region FSMVariables
    FiniteStateMachine fsm;

    [SerializeField] LRDemonIdleState idleState;
    [SerializeField] LRDemonChaseState chaseState;
    [SerializeField] LRDAttackState attackState;
    [SerializeField] LRDDemonMoveAroundState moveAroundState;
    [SerializeField] LRDemonBanishState banishState;
    [SerializeField] LRDemonDeath deathState;

    

    #endregion
        
    private void Start()
    {
        fsm = new FiniteStateMachine(idleState, StartCoroutine);

        //Idle
        fsm.AddTransition(StateTransitions.ToChase, idleState, chaseState);
        fsm.AddTransition(StateTransitions.ToAttack, idleState, attackState);
        fsm.AddTransition(StateTransitions.ToMoveAround, idleState, moveAroundState);
        fsm.AddTransition(StateTransitions.ToBanish, idleState, banishState);
        
        //Chase
        fsm.AddTransition(StateTransitions.ToAttack, chaseState, attackState);
        fsm.AddTransition(StateTransitions.ToMoveAround, chaseState, moveAroundState);
        fsm.AddTransition(StateTransitions.ToBanish, chaseState, banishState);
        
        //Attack
        fsm.AddTransition(StateTransitions.ToIdle, attackState, idleState);
        fsm.AddTransition(StateTransitions.ToBanish, attackState, banishState);

        //Move Around
        fsm.AddTransition(StateTransitions.ToChase, moveAroundState, chaseState);
        fsm.AddTransition(StateTransitions.ToAttack, moveAroundState, attackState);
        fsm.AddTransition(StateTransitions.ToBanish, moveAroundState, banishState);
        
        //Banish
        fsm.AddTransition(StateTransitions.ToDeath, banishState, deathState);

        fsm.Active = true;
    }

    private void Awake()
    {
        target = Player.Instance.transform;
        _animator = GetComponentInChildren<LRDemonAnim>();
    }

    private void Update()
    {
        if (_cdForAttack > 0)
        {
            _cdForAttack -= Time.deltaTime;
        }

        _ray = Physics.Raycast(transform.position,
            target.transform.position - transform.position,
            Vector3.Distance(target.transform.position,transform.position), 
            _layer);
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
        return _cdForAttack <= 0 && !_ray;
    }

    public bool canBanish { get; set; }
    public bool onBanishing { get; set; }

    public void StartBanish()
    {
        onBanishing = true;
    }

    public void FinishBanish()
    {
        
    }

    public void StartAttack()
    {
        hitboxAttack.SetActive(true);
    }

    public void FinishAttack()
    {
        hitboxAttack.SetActive(false);
    }
}
