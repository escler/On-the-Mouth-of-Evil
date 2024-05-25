using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemon : EnemySteeringAgent
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
    [SerializeField] public Transform _model;
    public int enemyCount;
    public bool canHit, enemyHit;
    public float waitForHitAgain;
    public GameObject fireBall, floorAttack;
    public Vector3[] points = new Vector3[4];

    public float speedWalk, speedRun;
    
    
    private IllusionDemonAnim _anim;

    [SerializeField] private DecisionNode _decisionTree;
    public DecisionNode DecisionTree => _decisionTree;

    public float CdForAttack
    {
        get { return _cdForAttack; }
        set { _cdForAttack = value; }
    }

    public Transform CharacterPos => _characterPos;
    public IllusionDemonAnim Anim => _anim;
    void Awake()
    {
        _anim = GetComponentInChildren<IllusionDemonAnim>();
        _characterPos = Player.Instance.transform;
        _fsm = new FiniteStateMachine();
        
        _fsm.AddState(States.Idle, new IllusionDemon_Idle(this));
        _fsm.AddState(States.Moving, new IllusionDemon_Moving(this));
        _fsm.AddState(States.Hit, new IllusionDemon_Hit(this));
        _fsm.AddState(States.Attack, new IllusionDemon_ComboHit(this));
        
        _fsm.ChangeState(States.Moving);
        EnemyManager.Instance.AddEnemy(this);
        ListDemonsUI.Instance.AddText(enemyCount, "Demon " + enemyCount);
        canHit = true;
    }

    public void ChangeToMove()
    {
        _fsm.ChangeState(States.Moving);
    }

    public void ChangeToHit()
    {
        _fsm.ChangeState(States.Hit);
    }
    
    public void ChangeToCombo()
    {
        _fsm.ChangeState(States.Attack);
    }
    private void Update()
    {
        _fsm.OnUpdate();
        if(enemyHit) ChangeToHit();
    }
}
