using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadens : EnemySteeringAgent
{
    private FiniteStateMachineWithoutInputs _fsm;
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
    [SerializeField] public Transform _model;
    public int enemyCount;
    public bool canHit, enemyHit;
    public float waitForHitAgain;
    public GameObject fireBall, floorAttack;
    public Vector3[] points = new Vector3[4];
    public bool summonedByBoss;

    private MageAnim _mageAnim;

    [SerializeField] private DecisionNode _decisionTree;
    public DecisionNode DecisionTree => _decisionTree;

    public float CdForAttack
    {
        get { return _cdForAttack; }
        set { _cdForAttack = value; }
    }

    public Transform CharacterPos => _characterPos;
    public MageAnim mageAnim => _mageAnim;
    void Awake()
    {
        _mageAnim = GetComponentInChildren<MageAnim>();
        _characterPos = GameObject.Find("Player").GetComponent<Transform>();
        _fsm = new FiniteStateMachineWithoutInputs();
        
        _fsm.AddState(States.Idle, new Idle(this));
        _fsm.AddState(States.Attack, new AttackMage(this));
        _fsm.AddState(States.FloorAttack, new FloorAttack(this));
        _fsm.AddState(States.Moving, new Moving(this));
        _fsm.AddState(States.Hit, new Hit(this));
        
        _fsm.ChangeState(States.Idle);
        ListDemonsUI.Instance.AddText(enemyCount, "Demon " + enemyCount);
        canHit = true;
    }

    void Update()
    {
        if (!canHit)
        {
            waitForHitAgain -= Time.deltaTime;
            if (waitForHitAgain <= 0)
            {
                canHit = true;
            }
        }
        transform.LookAt(new Vector3(_characterPos.position.x, transform.position.y, _characterPos.position.z));
        _fsm?.OnUpdate();
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
        _obstacleWithPlayer = Physics.Raycast(transform.position, (finalPos - transform.position).normalized,
            (finalPos - transform.position).magnitude, _obstacles);
        return _obstacleWithPlayer;
    }

    public void IdleState()
    {
        _fsm.ChangeState(States.Idle);
    }

    public void MovingSate()
    {        
        _fsm.ChangeState(States.Moving);
    }

    public void AttackState()
    {
        _fsm.ChangeState(States.Attack);
    }
    
    public void FloorState()
    {
        _fsm.ChangeState(States.FloorAttack);
    }
    
    public void HitState()
    {
        _fsm.ChangeState(States.Hit);
    }

    public void Arrive()
    {
        if (!HastToUseObstacleAvoidance(transform.localScale.x))
        {
            AddForce(Arrive(_characterPos.position));
        }
        Move();

    }

    public void Attack()
    {
        Instantiate(fireBall, _attackSpawn.position, _attackSpawn.rotation);
    }

    public void FloorAttack()
    {
        Instantiate(floorAttack,
            new Vector3(_characterPos.position.x, transform.position.y + 0.1f, 
                _characterPos.position.z), transform.rotation);
    }

    public bool CheckCollider()
    {
        for (int i = 0; i < points.Length; i++)
        {
            bool ray = Physics.Raycast(transform.position + transform.up, points[i], 2, _obstacles);

            if (ray)
            {
                return true;
            }
        }

        return false;
    }
    
    public void MoveChar()
    {
        for (int i = 0; i < points.Length; i++)
        {
            bool ray = Physics.Raycast(transform.position + transform.up, points[i], 2, _obstacles);

            if (ray)
            {
                transform.position -= points[i];
            }
        }
    }
}
