using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Schema;

public class IllusionDemon : EnemySteeringAgent
{
    private FiniteStateMachine _fsm;
    private bool _obstacleWithPlayer, _playerInFov;
    private float _cdForAttack;
    private Transform _characterPos;
    [SerializeField] private Transform _attackSpawn;
    public float timeForChannelAttack, rangeForSpecialAttack;
    public GameObject spawnHitbox;
    [SerializeField] public Transform _model;
    public bool canHit, enemyHit, finishCast;
    public int hitCount;

    public GameObject lowRangeDemons, copiesGO, copiesFightGO, explosionCopies;
    public GameObject copy1, copy2;

    public float speedWalk, speedRun;
    public int enemiesCount, fightingCopies;
    private IllusionDemonAnim _anim;
    public actionsEnemy lastAction;
    public Actions lastActionAttack;

    public BossZoneManager _zoneManager;
    [SerializeField] private DecisionNode _decisionTree;

    [Header("Cast Spell")] 
    public GameObject startCast;
    public GameObject spell;
    public Transform pivot;

    [Header("Fog Attack")] 
    public int copiesPerAttack;
    public int actualCopies;
    public bool copyAlive;

    public GameObject[] fogEnemies;

    public DecisionNode DecisionTree => _decisionTree;

    public Transform CharacterPos => _characterPos;
    public IllusionDemonAnim Anim => _anim;

    void Awake()
    {
        _anim = GetComponentInChildren<IllusionDemonAnim>();
        _characterPos = Player.Instance.transform;
        _zoneManager = BossZoneManager.Instance;
        _fsm = new FiniteStateMachine();

        _fsm.AddState(States.Idle, new IllusionDemon_Idle(this));
        _fsm.AddState(States.Moving, new IllusionDemon_Moving(this));
        _fsm.AddState(States.Hit, new IllusionDemon_Hit(this));
        _fsm.AddState(States.Attack, new IllusionDemon_ChannelAttack(this));
        _fsm.AddState(States.SpecialAttack, new IllusionDemon_JumpAttack(this));
        _fsm.AddState(States.CastAttack, new IllusionDemon_FogAttack(this));

        _fsm.ChangeState(States.Idle);

        CreateCopies();
    }

    public void ChangeToIdle()
    {
        _fsm.ChangeState(States.Idle);
    }

    public void ChangeToMove()
    {
        _fsm.ChangeState(States.Moving);
    }

    public void ChangeToHit()
    {
        _fsm.ChangeState(States.Hit);
    }

    public void ChangeToCastAttack()
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

    public void ChangeToDuplicationFight()
    {
        _fsm.ChangeState(States.BossDuplicationCopy);
    }

    private void Update()
    {
        _fsm.OnUpdate();
        enemyHit = hitCount >= 3;
        if (enemyHit) ChangeToHit();
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

    public Vector3 NewLocation()
    {
        var furthestLocation = _zoneManager.points.OrderBy(x => Vector3.Distance(_characterPos.position, x.position))
            .Last().position;

        return new Vector3(furthestLocation.x, transform.position.y, furthestLocation.z);
    }

    public Vector3 LocationForJumpAttack()
    {
        var nearestLocation = _zoneManager.points.OrderBy(x => Vector3.Distance(_characterPos.position, x.position))
            .SkipWhile(x => Vector3.Distance(_characterPos.position, x.position) < rangeForSpecialAttack).First()
            .position;
        return new Vector3(nearestLocation.x, transform.position.y, nearestLocation.z);
    }

    public void SpawnExplosionCopies(float xMin, float xMax)
    {
        var bounds = _zoneManager.GetComponent<BoxCollider>().bounds;
        var validPosCenter = _zoneManager.transform.position;
        var explosionCopy = Instantiate(explosionCopies);
        var posX = Random.Range(_characterPos.position.x + xMin, _characterPos.position.x + xMax);
        posX = Mathf.Clamp(posX, validPosCenter.x - bounds.extents.x, validPosCenter.x + bounds.extents.x);
        var posZ = Random.Range(_characterPos.position.z - 6, _characterPos.position.z - 10);
        posZ = Mathf.Clamp(posZ, validPosCenter.z - bounds.extents.z, validPosCenter.z + bounds.extents.z);
        explosionCopy.transform.position = new Vector3(posX, transform.position.y, posZ);
    }

    public void FireSpell()
    {
        Instantiate(spell, pivot.position, transform.rotation);
    }

    public void InvokeCopies()
    {
        copy1.SetActive(true);
        copy1.transform.position = transform.position - transform.right * 4;
        copy1.transform.rotation = transform.rotation;

        copy2.SetActive(true);
        copy2.transform.position = transform.position + transform.right * 4;
        copy2.transform.rotation = transform.rotation;
    }

    public void SwitchPosition()
    {
        Anim.Animator.applyRootMotion = false;
        var condition = Random.Range(1, 3);
        var bossPos = transform.position;
        var bossRot = transform.rotation;
        var copyPos = copy1.transform.position;
        var copyRot = copy1.transform.rotation;
        var copyPos2 = copy2.transform.position;
        var copyRot2 = copy2.transform.rotation;
        switch (condition)
        {
            case 0:
                break;
            case 1:
                copy1.transform.position = bossPos;
                copy1.transform.rotation = bossRot;
                transform.position = copyPos;
                transform.rotation = copyRot;
                break;
            case 2:
                copy2.transform.position = bossPos;
                copy2.transform.rotation = bossRot;
                transform.position = copyPos2;
                transform.rotation = copyRot2;
                break;
        }
    }

    public void StartFogAttack()
    {
        StartCoroutine(SpawnCopyFog(fogEnemies, -5, 5));
    }

    public void EndFogAttack()
    {
        StopCoroutine(SpawnCopyFog(fogEnemies,-5,5));
    }

    IEnumerator SpawnCopyFog(GameObject[] copies, float xMin, float xMax)
    {
        while (actualCopies > 0)
        {
            var copy = Random.Range(0, copies.Length);
            copyAlive = true;
            var bounds = _zoneManager.GetComponent<BoxCollider>().bounds;
            var validPosCenter = _zoneManager.transform.position;
            var posX = Random.Range(_characterPos.position.x + xMin, _characterPos.position.x + xMax);
            posX = Mathf.Clamp(posX, validPosCenter.x - bounds.extents.x, validPosCenter.x + bounds.extents.x);
            var posZ = Random.Range(_characterPos.position.z - 6, _characterPos.position.z - 10);
            posZ = Mathf.Clamp(posZ, validPosCenter.z - bounds.extents.z, validPosCenter.z + bounds.extents.z);

            Instantiate(copies[copy], new Vector3(posX, transform.position.y, posZ), transform.rotation);
            yield return new WaitUntil(() => copyAlive == false);
        }
    }
}

public enum actionsEnemy
{
    NotAttack,
    Attack
}
