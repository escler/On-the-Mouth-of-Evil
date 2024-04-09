using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadens : SteeringAgent
{
    private FiniteStateMachine _fsm;
    public bool CanAttack => _cdForAttack <= 0;
    public bool InRangeForAttack => Vector3.Distance(_characterPos.position, transform.position) <= rangeForAttack;
    private bool _obstacleWithPlayer, _playerInFov;
    public bool PlayerInFov => _playerInFov;
    public float initialCdForAttack;
    private float _cdForAttack;
    private Transform _characterPos;
    [SerializeField] private Transform _attackSpawn;
    public float rangeForAttack;
    [SerializeField] private MeleeWeapon _weapon;

    [SerializeField] private DecisionNode _decisionTree;
    public DecisionNode DecisionTree => _decisionTree;

    public float CdForAttack
    {
        get { return _cdForAttack; }
        set { _cdForAttack = value; }
    }

    public Transform CharacterPos => _characterPos;
    void Awake()
    {
        _characterPos = GameObject.Find("Player").GetComponent<Transform>();
        _fsm = new FiniteStateMachine();
        
        _fsm.AddState(States.Idle, new Idle(this));
        _fsm.AddState(States.Attack, new Attack(this));
        _fsm.AddState(States.Chase, new Chase(this));
        _fsm.AddState(States.PfCharacter, new PfCharacter());
        
        _fsm.ChangeState(States.Idle);
    }

    void Update()
    {
        _fsm.OnUpdate();
    }

    public void InFieldOfView()
    {
        var endPos = _characterPos.transform.position;
            
        Vector3 dir = endPos - transform.position;
        if (dir.magnitude <= _viewRadius && InLineOfSight(transform.position, endPos) &&
            Vector3.Angle(transform.forward, dir) <= _viewAngle / 2)
        {
            _playerInFov = true;
        }
        else
        {
            _playerInFov = false;
        }
    }
    
    public bool InLineOfSight(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - start;
        return !Physics.Raycast(start, dir, dir.magnitude, _obstacles);
    }
    
    public bool LineOfSight()
    {
        Vector3 finalPos = _characterPos.position;
        _obstacleWithPlayer = Physics.Raycast(transform.position, (finalPos- transform.position).normalized,
            (finalPos - transform.position).magnitude, _obstacles);
        return _obstacleWithPlayer;
    }

    public void IdleState()
    {
        _fsm.ChangeState(States.Idle);
    }

    public void PfState()
    {        
        _fsm.ChangeState(States.PfCharacter);
    }

    public void ChaseState()
    {
        _fsm.ChangeState(States.Chase);
    }

    public void AttackState()
    {
        _fsm.ChangeState(States.Attack);
    }

    public void Arrive()
    {
        AddForce(Arrive(_characterPos.position));;
        Move();
    }

    public void Attack()
    {
        _weapon.SpawnHitBox();
    }
}
