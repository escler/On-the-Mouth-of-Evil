using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using Unity.VisualScripting;

public class DemonLowRange : Enemy
{
    public Transform target;

    [SerializeField] float distanceToAttack = 3;
    [SerializeField] float distanceToPersuit = 15;
    private float _cdForAttack;
    public LayerMask layer;
    private bool _ray;
    public bool banished;
    public int hitNeeded;
    private int _actualHit;

    public int ActualHit
    {
        set => _actualHit = value;
    }
    public bool cantHit;
    [SerializeField] private GameObject hitboxAttack;

    public float cdForAttack;
    private LRDemonAnim _animator;
    public LRDemonAnim animator => _animator;

    private SpatialGrid _spatial;
    private bool _cantChangeDirection;
    public bool CantChangeDirection { set => _cantChangeDirection = value; }
    private float _dotX;
    private float _dotY;
    public float DotX => _dotX;
    public float DotY => _dotY;

    public int force;

    #region FSMVariables
    FiniteStateMachine fsm;

    [SerializeField] LRDemonIdleState idleState;
    [SerializeField] LRDemonChaseState chaseState;
    [SerializeField] LRDAttackState attackState;
    [SerializeField] LRDDemonMoveAroundState moveAroundState;
    [SerializeField] LRDemonBanishState banishState;
    [SerializeField] LRDemonDeath deathState;
    [SerializeField] LRDemonReactHit hitState;
    
    #endregion
        
    private void Start()//IA2-P3
    {
        fsm = new FiniteStateMachine(idleState, StartCoroutine);

        //Idle
        fsm.AddTransition(StateTransitions.ToChase, idleState, chaseState);
        fsm.AddTransition(StateTransitions.ToAttack, idleState, attackState);
        fsm.AddTransition(StateTransitions.ToMoveAround, idleState, moveAroundState);
        fsm.AddTransition(StateTransitions.ToBanish, idleState, banishState);
        fsm.AddTransition(StateTransitions.ToHit, idleState, hitState);
        
        //Chase
        fsm.AddTransition(StateTransitions.ToAttack, chaseState, attackState);
        fsm.AddTransition(StateTransitions.ToMoveAround, chaseState, moveAroundState);
        fsm.AddTransition(StateTransitions.ToBanish, chaseState, banishState);
        fsm.AddTransition(StateTransitions.ToHit, chaseState, hitState);
        
        //Attack
        fsm.AddTransition(StateTransitions.ToIdle, attackState, idleState);
        fsm.AddTransition(StateTransitions.ToBanish, attackState, banishState);
        fsm.AddTransition(StateTransitions.ToHit, attackState, hitState);


        //Move Around
        fsm.AddTransition(StateTransitions.ToChase, moveAroundState, chaseState);
        fsm.AddTransition(StateTransitions.ToAttack, moveAroundState, attackState);
        fsm.AddTransition(StateTransitions.ToBanish, moveAroundState, banishState);
        fsm.AddTransition(StateTransitions.ToHit, moveAroundState, hitState);

        
        //Banish
        fsm.AddTransition(StateTransitions.ToDeath, banishState, deathState);
        
        //Hit
        fsm.AddTransition(StateTransitions.ToIdle, hitState, idleState);

        fsm.Active = true;
    }

    private void Awake()
    {
        OnAwake();
        target = Player.Instance.transform;
        _animator = GetComponentInChildren<LRDemonAnim>();
    }

    private void OnEnable()
    {
        EnemyManager.Instance.AddEnemy(this);
        _spatial = GameManager.Instance.activeSpatialGrid;
        _spatial.Add(this);
        EnemyMove();
    }

    private void OnDisable()
    {
        EnemyManager.Instance.AddEnemy(this);
        GameManager.Instance.activeZoneManager.EnemyDead();
        _spatial.Remove(this);
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
            layer);
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
    private void ResultOfBanish()
    {
        banished = TypeManager.Instance.ResultOfType();
        FinishBanish();
    }
    
    public override void StartBanish()
    {
        TypeManager.Instance.onResult += ResultOfBanish;
        BanishManager.Instance.CreateNewBanishLine(transform.position);
        onBanishing = true;
    }

    public bool ReactHit()
    {
        return _actualHit >= hitNeeded;
    }

    public override void FinishBanish()
    {
        TypeManager.Instance.onResult -= ResultOfBanish;
        onBanishing = false;
    }

    public void StartAttack()
    {
        hitboxAttack.SetActive(true);
    }

    public void FinishAttack()
    {
        hitboxAttack.SetActive(false);
    }

    public void EntityMove()
    {
        EnemyMove();
    }

    public void AddHitCount(int count)
    {
        if (cantHit) return;
        _actualHit += count;
        if (_actualHit < hitNeeded || _cantChangeDirection) return;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 toOther = Vector3.Normalize(transform.position - target.position);
        Vector3 right = transform.TransformDirection(Vector3.right);
        _dotX = Mathf.Round(Vector3.Dot(forward,toOther));
        _dotY = MathF.Round(Vector3.Dot(right,toOther));
        _cantChangeDirection = true;
    }
}
