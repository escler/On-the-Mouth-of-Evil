using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
using FSM;


public class IllusionDemon : Enemy
{ 
    public Action OnBossDefeated;
    
    private Transform _characterPos;
    [SerializeField] private Transform _attackSpawn;
    public float timeForChannelAttack, rangeForJumpAttack;
    public GameObject spawnHitbox;
    [SerializeField] public Transform _model;
    public bool canHit, enemyHit, finishCast;
    public int hitCount;

    public GameObject copiesGO, explosionCopies;
    public GameObject copy1, copy2;

    public float speedWalk, speedRun;
    public IllusionDemonAnim _anim;
    public string lastsStateTransitionAttack;

    public BossZoneManager _zoneManager;

    [Header("Cast Spell")] 
    public GameObject startCast;
    public GameObject spell;
    public Transform pivot;

    [Header("Fog Attack")] 
    public int copiesPerAttack;
    public int actualCopies;
    public bool copyAlive;
    public bool firstPhase, secondPhase;

    [Header("Throw Objects")] 
    public Transform throwObjectPos;
    public ThrowItem actualItem;

    public GameObject[] fogEnemies;
    
    public Transform CharacterPos => _characterPos;
    public IllusionDemonAnim Anim => _anim;
    
    public float timeToBanish;
    public GameObject banishPS;

    public AudioSource bossAudioSource;
    public AudioClip tpSound;
    public AudioClip throwItemSound;

    public GameObject psTrail;
    
    #region FSM
    FiniteStateMachine fsm;

    [SerializeField] IllusionDemon_Idle idleState;
    [SerializeField] IllusionDemon_Moving moveAroundState;
    [SerializeField] IllusionDemon_Hit hitState;
    [SerializeField] IllusionDemon_FogAttack fogAttackState;
    [SerializeField] IllusionDemon_ChannelAttack channelAttackState;
    [SerializeField] IllusionDemon_JumpAttack jumpAttackState;
    [SerializeField] IllusionDemon_ThrowObjects throwAttackState;
    [SerializeField] IllusionDemon_Banish banishState;
    [SerializeField] IllusionDemon_Death deathState;
    
    #endregion

    private void Start() //IA2-P3
    {
        fsm = new FiniteStateMachine(idleState, StartCoroutine);
        
        //Idle
        fsm.AddTransition(StateTransitions.ToMoveAround, idleState, moveAroundState);
        fsm.AddTransition(StateTransitions.ToBanish, idleState, banishState);

        //Moving
        fsm.AddTransition(StateTransitions.ToFogAttack, moveAroundState, fogAttackState);
        fsm.AddTransition(StateTransitions.ToChannelAttack, moveAroundState, channelAttackState);
        fsm.AddTransition(StateTransitions.ToJumpAttack, moveAroundState, jumpAttackState);
        fsm.AddTransition(StateTransitions.ToThrowAttack, moveAroundState, throwAttackState);
        fsm.AddTransition(StateTransitions.ToBanish, moveAroundState, banishState);
        
        //Hit
        fsm.AddTransition(StateTransitions.ToIdle, hitState, idleState);
        fsm.AddTransition(StateTransitions.ToBanish, hitState, banishState);
        
        //FogAttack
        fsm.AddTransition(StateTransitions.ToIdle, fogAttackState, idleState);
        fsm.AddTransition(StateTransitions.ToBanish, fogAttackState, banishState);
        
        //ChannelAttack
        fsm.AddTransition(StateTransitions.ToIdle, channelAttackState, idleState);
        fsm.AddTransition(StateTransitions.ToHit, channelAttackState, hitState);
        fsm.AddTransition(StateTransitions.ToBanish, channelAttackState, banishState);
        
        //JumpAttack
        fsm.AddTransition(StateTransitions.ToIdle, jumpAttackState, idleState);
        fsm.AddTransition(StateTransitions.ToBanish, jumpAttackState, banishState);
        
        //ThrowAttack
        fsm.AddTransition(StateTransitions.ToIdle, throwAttackState, idleState);
        fsm.AddTransition(StateTransitions.ToBanish, throwAttackState, banishState);
        
        //Banish
        fsm.AddTransition(StateTransitions.ToIdle, banishState, idleState);
        fsm.AddTransition(StateTransitions.ToDeath, banishState, deathState);
        fsm.Active = true;
    }

    void Awake()
    {
        OnAwake();
        GameManager.Instance.activeSpatialGrid.Add(this);
        EnemyIsMoving();
        
        _anim = GetComponentInChildren<IllusionDemonAnim>();
        _characterPos = Player.Instance.transform;
        _zoneManager = BossZoneManager.Instance;
        
        CreateCopies();
    }

    private void OnDestroy()
    {
        GameManager.Instance.activeSpatialGrid.Remove(this);
    }
    
    private void Update()
    {
        if(canHit) enemyHit = hitCount >= 3;
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

    public Vector3 NewLocation() //IA2-P1
    {
        var furthestLocation = _zoneManager.points.Aggregate(_zoneManager.points.First().position, (acum, current) =>
        {
            if (Vector3.Distance(current.position, CharacterPos.position) 
                > Vector3.Distance(acum, CharacterPos.position)) return current.position;
                
            return acum;
        });
        CreateTrail();
        return new Vector3(furthestLocation.x, transform.position.y, furthestLocation.z);
    }

    private void CreateTrail()
    {
        var currentTrail = Instantiate(psTrail, transform.position, transform.rotation);
        currentTrail.GetComponent<FollowTarget>().GetTarget(transform);
    }

    public Vector3 LocationForJumpAttack() //IA2-P1
    {
        var nearestLocation = _zoneManager.points
            .Aggregate(_zoneManager.points.OrderBy(x => Vector3.Distance(_characterPos.position,x.position))
            .Last().position, (acum, current) =>
        {
            var currentDistance = Vector3.Distance(current.position, CharacterPos.position);
            if (currentDistance <
                Vector3.Distance(acum, CharacterPos.position) && currentDistance > rangeForJumpAttack + .2f) acum = current.position;
            return acum;
        });
        CreateTrail();
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
        startCast.SetActive(false);
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
        Player.Instance.sphere.GetComponent<FogPlayer>().start = true;
        yield return new WaitForSeconds(2f);
        
        while (actualCopies > 0)
        {
            var copy = Random.Range(0, copies.Length);
            copyAlive = true;
            var bounds = _zoneManager.GetComponent<BoxCollider>().bounds;
            var validPosCenter = _zoneManager.transform.position;
            var posZ = 1f;
            posZ = Mathf.Clamp(posZ, validPosCenter.z - bounds.extents.z, validPosCenter.z + bounds.extents.z);
            var characterFront = _characterPos.position - _characterPos.forward * posZ;
            Instantiate(copies[copy], new Vector3(characterFront.x, transform.position.y, characterFront.z), transform.rotation);
            yield return new WaitUntil(() => copyAlive == false);
        }
        Player.Instance.PossesControls();
    }

    public Vector3 MoveBoss()
    {
        var bounds = _zoneManager.GetComponent<BoxCollider>().bounds;
        var validPosCenter = _zoneManager.transform.position;
        var posZ = 1f;
        posZ = Mathf.Clamp(posZ, validPosCenter.z - bounds.extents.z, validPosCenter.z + bounds.extents.z);

        var characterFront = _characterPos.position - _characterPos.forward * posZ;
        return new Vector3(characterFront.x, transform.position.y, characterFront.z);
    }

    public void RestoreLife()
    {
        GetComponent<IllusionDemonLifeHandler>().RechargeLife();
    }

    public void MoveObject()
    {
        actualItem.SetLocation(throwObjectPos.position, gameObject);
    }

    public void ThrowObject()
    {
        actualItem.ThrowObject(_characterPos.position);
        bossAudioSource.PlayOneShot(throwItemSound);
        actualItem._callBackHit = true;
    }

    public void ResultOfBanish()
    {
        if (TypeManager.Instance.ResultOfType())
        {
            banished = true;
        }
        FinishBanish();
    }
    

    public override void StartBanish()
    {
        TypeManager.Instance.onResult += ResultOfBanish;
        BanishManager.Instance.CreateNewBanishLine(transform.position);
        onBanishing = true;
    }

    public override void FinishBanish()
    {
        onBanishing = false;
        TypeManager.Instance.onResult -= ResultOfBanish;
    }
    
    public void EnemyIsMoving()
    {
        EnemyMove();
    }

    public void DisableFSM()
    {
        fsm.Active = false;
    }

    public void MakeTpSound()
    {
        bossAudioSource.PlayOneShot(tpSound);
    }
}

public enum actionsEnemy
{
    NotAttack,
    Attack
}
