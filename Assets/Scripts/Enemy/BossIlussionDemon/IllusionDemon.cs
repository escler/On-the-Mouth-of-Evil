using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public float rangeForAttack, rangeForSpecialAttack;
    public GameObject spawnHitbox;
    [SerializeField] public Transform _model;
    public int enemyCount;
    public bool canHit, enemyHit;
    public float waitForHitAgain;
    public GameObject fireBall, floorAttack;
    public Vector3[] points = new Vector3[4];

    public GameObject lowRangeDemons;

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
        _fsm.AddState(States.SpecialAttack, new IllusionDemon_JumpAttack(this));
        _fsm.AddState(States.CastAttack, new IllusionDemon_Cast(this));
        
        _fsm.ChangeState(States.Moving);
        EnemyManager.Instance.AddEnemy(this);
        ListDemonsUI.Instance.AddText(enemyCount, "Illusion Demon");
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

    public void ChangeToSpecialAttack()
    {
        _fsm.ChangeState(States.SpecialAttack);
    }

    public void ChangeCastAttack()
    {
        _fsm.ChangeState(States.CastAttack);
    }
    private void Update()
    {
        _fsm.OnUpdate();
        if(enemyHit) ChangeToHit();
    }

    public void InvokeDemon()
    {
        var offsetX = Random.Range(-6, 6);
        var offsetZ = Random.Range(-6, 6);

        Vector3 posToDemon = new Vector3(transform.position.x + offsetX, transform.position.y,
            transform.position.z + offsetZ);

        var demonSpawned = Instantiate(lowRangeDemons, posToDemon, transform.rotation);

        demonSpawned.GetComponent<SpawnEnemy>().SpawnWithDelay();
    }
}
