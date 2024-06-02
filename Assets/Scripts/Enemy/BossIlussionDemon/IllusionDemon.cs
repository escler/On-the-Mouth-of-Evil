using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IllusionDemon : EnemySteeringAgent
{
    private FiniteStateMachine _fsm;
    private bool _obstacleWithPlayer, _playerInFov;
    private float _cdForAttack;
    private Transform _characterPos;
    [SerializeField] private Transform _attackSpawn;
    public float rangeForAttack, rangeForSpecialAttack;
    public GameObject spawnHitbox;
    [SerializeField] public Transform _model;
    public bool canHit, enemyHit, finishCast;
    public Transform[] spawns;

    public GameObject lowRangeDemons, copiesGO, copiesFightGO;
    public GameObject copy1, copy2;

    public float speedWalk, speedRun;
    public int enemiesCount, fightingCopies;
    private IllusionDemonAnim _anim;
    public actionsEnemy lastAction;
    public Actions lastActionAttack;
    [SerializeField] private DecisionNode _decisionTree;
    public DecisionNode DecisionTree => _decisionTree;

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
        
        _fsm.ChangeState(States.Idle);
        EnemyManager.Instance.AddEnemy(this);
        ListDemonsUI.Instance.AddText(0, "Illusion Demon");
        canHit = true;
        
        CreateCopies();
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

        demonSpawned.GetComponent<SpawnEnemy>().SpawnWithDelay(true);
        enemiesCount++;
    }

    private void CreateCopies()
    {
        var c1 = Instantiate(copiesGO);
        copy1 = c1;
        copy1.SetActive(false);
        var c2 = Instantiate(copiesGO, transform.position - transform.right, transform.rotation);
        copy2 = c2;
        copy2.SetActive(false);
    }
    public void InvokeCopies()
    {
        copy1.SetActive(true);
        copy1.transform.position = transform.position - transform.right * 2;
        copy1.transform.rotation = transform.rotation;
            
        copy2.SetActive(true);
        copy2.transform.position = transform.position + transform.right * 2;
        copy2.transform.rotation = transform.rotation;
    }

    public void InvokeFightCopies()
    {
        for (int i = 0; i < spawns.Length - 1; i++)
        {
            Instantiate(copiesFightGO, spawns[i].position, spawns[i].rotation);
            fightingCopies++;
        }
    }
}

public enum actionsEnemy
{
    NotAttack,
    Attack
}
