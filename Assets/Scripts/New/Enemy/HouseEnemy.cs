using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FSM;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class HouseEnemy : Enemy
{
    public static HouseEnemy Instance;
    public float speed;
    public LayerMask obstacles;
    public PathFinding pf;
    public Room actualRoom, crossRoom;
    public bool crossUsed; 
    private PlayerHandler _player;
    public float actualTime, timeToShowMe;
    private List<IInteractableEnemy> objects;
    private bool canInteract;
    public SkinnedMeshRenderer mesh;
    PlayParticles Fire;
    public bool appear;
    [SerializeField] public GameObject lavaPrefab;
    private Vector3 targetScale;

    private FiniteStateMachine _fsm;
    [SerializeField] private HouseEnemy_Spawn spawnState;
    [SerializeField] private HouseEnemy_Idle idleState;
    [SerializeField] private HouseEnemy_Patrol patrolState;
    [SerializeField] private HouseEnemy_Attacks attacksState;
    [SerializeField] private HouseEnemy_GoToLocation goToLocationState;
    [SerializeField] private HouseEnemy_Ritual ritualState;
    [SerializeField] private HouseEnemy_GrabHead grabHeadState;
    [SerializeField] private HouseEnemy_Voodoo voodooState;
    private bool hasPlayedFire;
    public bool ritualDone;
    public Node nodeRitual;

    public Material enemyMaterial;
    public float enemyVisibility;
    private bool _corroutineActivate;

    public bool onRitual;

    private CorduraHandler _corduraHandler;


    private HouseEnemyView _enemyAnimator;

    public HouseEnemyView EnemyAnimator => _enemyAnimator;

    public bool grabHead;
    public Transform headPos;

    public bool attackEnded;

    public bool enemyVisible, canAttackPlayer, compareRoom;
    public float actualTimeToLost;
    public bool activateGoodExorcism, activateBadExorcism;

    public Transform obstaclePosRight, obstaclePosLeft;
    public float distanceObstacleRay;
    public float intensityObstacleAvoidance;
    public Vector3 reference = Vector3.zero;
    public CapsuleCollider capsule;
    public float rotationSmoothTime;

    public int playerGrabbedCount;
    public ParticleSystem[] smokePS;

    public GameObject absorbVFX, magnetVFX;

    public GameObject normalMesh, ritualMesh;

    private float _position;

    public bool voodooActivate;
    public Vector3 voodooPosition;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        capsule.GetComponent<CapsuleCollider>();
        _corduraHandler = CorduraHandler.Instance;
        _enemyAnimator = GetComponentInChildren<HouseEnemyView>();
        enemyMaterial.SetFloat("_Power", 0);
        enemyVisibility = enemyMaterial.GetFloat("_Power");
        
        enemyMaterial.SetFloat("_Position", -0.63f);
        enemyVisibility = enemyMaterial.GetFloat("_Position");


        objects = new List<IInteractableEnemy>();
        _player = PlayerHandler.Instance;
        pf = new PathFinding();
        _fsm = new FiniteStateMachine(idleState, StartCoroutine);

        //Spawn
        _fsm.AddTransition(StateTransitions.ToAttacks, spawnState, attacksState);
        _fsm.AddTransition(StateTransitions.ToIdle, spawnState, idleState);
        _fsm.AddTransition(StateTransitions.ToVoodoo, spawnState, voodooState);
        
        
        //Idle
        _fsm.AddTransition(StateTransitions.ToPatrol, idleState, patrolState);
        _fsm.AddTransition(StateTransitions.ToAttacks, idleState, attacksState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, idleState, goToLocationState);
        _fsm.AddTransition(StateTransitions.ToRitual, idleState, ritualState);
        _fsm.AddTransition(StateTransitions.ToSpawn, idleState, spawnState);
        _fsm.AddTransition(StateTransitions.ToVoodoo, idleState, voodooState);
        
        //Patrol
        _fsm.AddTransition(StateTransitions.ToIdle, patrolState, idleState);
        _fsm.AddTransition(StateTransitions.ToAttacks, patrolState, attacksState);
        _fsm.AddTransition(StateTransitions.ToPatrol, patrolState, patrolState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, patrolState, goToLocationState);
        _fsm.AddTransition(StateTransitions.ToRitual, patrolState, ritualState);
        _fsm.AddTransition(StateTransitions.ToVoodoo, patrolState, voodooState);
        
        //Attack
        _fsm.AddTransition(StateTransitions.ToIdle, attacksState, idleState);
        _fsm.AddTransition(StateTransitions.ToPatrol, attacksState, patrolState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, attacksState, goToLocationState);
        _fsm.AddTransition(StateTransitions.ToRitual, attacksState, ritualState);
        _fsm.AddTransition(StateTransitions.ToAttacks, attacksState, attacksState);
        _fsm.AddTransition(StateTransitions.ToVoodoo, attacksState, voodooState);

        
        //GoToLocation
        _fsm.AddTransition(StateTransitions.ToSpawn, goToLocationState, spawnState);
        _fsm.AddTransition(StateTransitions.ToIdle, goToLocationState, idleState);
        _fsm.AddTransition(StateTransitions.ToPatrol, goToLocationState, patrolState);
        _fsm.AddTransition(StateTransitions.ToSpecifyLocation, goToLocationState, goToLocationState);
        _fsm.AddTransition(StateTransitions.ToRitual, goToLocationState, ritualState);
        _fsm.AddTransition(StateTransitions.ToVoodoo, goToLocationState, voodooState);

        //GoToGrabHead
        _fsm.AddTransition(StateTransitions.ToIdle, grabHeadState, idleState);
        
        //Voodoo
        _fsm.AddTransition(StateTransitions.ToPatrol, voodooState, patrolState);
        _fsm.AddTransition(StateTransitions.ToRitual, voodooState, ritualState);
        
        _fsm.Active = true;
        OnAwake();
        appear = false;
    }

    private void Update()
    {
        if (onRitual) return;
        CompareRooms();
        TimeToShow();

        if (actualTimeToLost > 0) actualTimeToLost -= Time.deltaTime;
        compareRoom = _player.actualRoom == actualRoom;
        canAttackPlayer = enemyVisible && actualTimeToLost > 0;
    }

    private void TimeToShow()
    {
        if (!compareRoom)
        {
            actualTime = 0;
        }
        else actualTime += Time.deltaTime;
    }

    public void HideEnemy()
    {
        if (_corroutineActivate) return;
        
        StartCoroutine(HideEnemyLerp());
        _enemyAnimator.ChangeStateAnimation("Spawn", false);
        appear = false;
    }

    public void ShowEnemyRitual()
    {
        if (!onRitual) Ritual();
        StopCoroutine(HideEnemyLerp());
    }

    void Ritual()
    {
        if (DecisionsHandler.Instance.badPath)
        {
            StartCoroutine(ShowEnemyOnBadRitual());
            StartCoroutine(MovePositionVariable());
        }
        else StartCoroutine(ShowEnemyOnGoodRitual());
        onRitual = true;

    }

    IEnumerator MovePositionVariable()
    {
        while (_position > -2.5f)
        {
            _position -= 0.0075f;
            enemyMaterial.SetFloat("_Position", _position);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator ShowEnemyOnBadRitual()
    {
        PlayerHandler.Instance.movement.ritualCinematic = true;
        Inventory.Instance.cantSwitch = true;
        yield return new WaitUntil(() => PlayerHandler.Instance.movement.inSpot);
        while (enemyVisibility < 8)
        {
            enemyVisibility += .45f;
            enemyMaterial.SetFloat("_Power", enemyVisibility);
            
            if (enemyVisibility >= 2f)
            {
                _enemyAnimator.ChangeStateAnimation("Absorb", true);
            }
            yield return new WaitForSeconds(0.1f);

        }
        Inventory.Instance.cantSwitch = true;

        yield return new WaitUntil(() => _enemyAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName("Absorb"));
        absorbVFX.SetActive(true);
        magnetVFX.SetActive(true);
        AbsorbsionShaderHandler.Instance.MakeShaderEffect();
        activateBadExorcism = true;

        var duration = _enemyAnimator.animator.GetCurrentAnimatorStateInfo(0).length;
        bool animPlayer = false;

        while (enemyVisibility > 0)
        {
            if (enemyVisibility < 6f && !animPlayer)
            {
                PlayerHandler.Instance.animator.enabled = true;
                PlayerHandler.Instance.animator.SetTrigger("LookAround");
                animPlayer = true;
            }
            enemyVisibility -= (8 / duration) * 0.0225f;
            enemyMaterial.SetFloat("_Power", enemyVisibility);
            yield return new WaitForSeconds(0.01f);
        }
        absorbVFX.GetComponent<VisualEffect>().Stop();
        magnetVFX.GetComponent<VisualEffect>().Stop();
        RitualManager.Instance.RitualFinish();

        if (PathManager.Instance.BadPath <= 0)
        {
            RitualManager.Instance.levitatingItems[1].gameObject.SetActive(true);
            RitualManager.Instance.actualItemActive = RitualManager.Instance.levitatingItems[1].gameObject;

        }
        yield return new WaitForSeconds(3f);
        StartCoroutine(GoToVoodo());
        //gameObject.SetActive(false);
    }

    IEnumerator GoToVoodo()
    {
        GameObject item = RitualManager.Instance.levitatingItems[1].gameObject;
        if (PathManager.Instance.BadPath > 0)
        {
            PlayerHandler.Instance.movement.ritualCinematic = false;
            PlayerHandler.Instance.animator.enabled = false;
            PathManager.Instance.ChangePrefs(DecisionsHandler.Instance.badPath ? "BadPath" : "GoodPath");
            GameManagerNew.Instance.LoadSceneWithDelay("Hub",5);
            yield break;
        }
        PlayerHandler.Instance.movement.absorbEnd = true;
        PlayerHandler.Instance.animator.enabled = false;
        PlayerHandler.Instance.movement.GoToVoodoo(item.transform.position);
        yield return new WaitUntil(() => PlayerHandler.Instance.movement.inVoodooPos);
        yield return new WaitForSeconds(2f);
        PlayerHandler.Instance.playerCam.CameraLock = true;
        item.GetComponent<LevitatingItem>().grabbed = true;
        Inventory.Instance.selectedItem.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        PlayerHandler.Instance.movement.ritualCinematic = false;
        PlayerHandler.Instance.movement.absorbEnd = false;
        
        PathManager.Instance.ChangePrefs(DecisionsHandler.Instance.badPath ? "BadPath" : "GoodPath");
        GameManagerNew.Instance.LoadSceneWithDelay("Hub",5);
        
    }
    
    IEnumerator ShowEnemyOnGoodRitual()
    {
        print("Entre a la cor Good Ritual");
        PlayerHandler.Instance.movement.ritualCinematic = true;
        Inventory.Instance.cantSwitch = true;
        yield return new WaitUntil(() => PlayerHandler.Instance.movement.inSpot);
        yield return new WaitUntil(() => enemyVisibility <= 0);
        
        Inventory.Instance.cantSwitch = true;
        
        while (enemyVisibility < 8)
        {
            enemyVisibility += .3f;
            enemyMaterial.SetFloat("_Power", enemyVisibility);

            yield return new WaitForSeconds(0.1f);
        }
        RitualManager.Instance.ritualFloor.SetActive(false);
        RitualManager.Instance.ActivateCraterFloor();

        yield return new WaitForSeconds(10f);
        
        while (enemyVisibility > 0)
        {
            enemyVisibility -= .3f;
            enemyMaterial.SetFloat("_Power", enemyVisibility);
            
            yield return new WaitForSeconds(0.1f);
        }

        RitualManager.Instance.godRayVFX.SetActive(true);
        PostProcessHandler.Instance.IncreaseExposure();
        RitualManager.Instance.CloseCrater();

        if (PathManager.Instance.GoodPath <= 0)
        {
            RitualManager.Instance.levitatingItems[0].gameObject.SetActive(true);
            RitualManager.Instance.actualItemActive = RitualManager.Instance.levitatingItems[0].gameObject;
        }
        yield return new WaitForSeconds(3f);
        StartCoroutine(GoToRosary());
    }
    
    IEnumerator GoToRosary()
    {
        print("Entre a la cor GoToRosary");
        GameObject item = RitualManager.Instance.levitatingItems[0].gameObject;
        if (PathManager.Instance.GoodPath > 0)
        {
            PlayerHandler.Instance.movement.ritualCinematic = false;
            PathManager.Instance.ChangePrefs(DecisionsHandler.Instance.badPath ? "BadPath" : "GoodPath");
            GameManagerNew.Instance.LoadSceneWithDelay("Hub",5);
            yield break;
        }
        PlayerHandler.Instance.movement.absorbEnd = true;
        PlayerHandler.Instance.movement.GoToVoodoo(item.transform.position);
        yield return new WaitUntil(() => PlayerHandler.Instance.movement.inVoodooPos);
        yield return new WaitForSeconds(2f);
        PlayerHandler.Instance.playerCam.CameraLock = true;
        item.GetComponent<LevitatingItem>().grabbed = true;
        Inventory.Instance.selectedItem.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        PlayerHandler.Instance.movement.ritualCinematic = false;
        PlayerHandler.Instance.movement.absorbEnd = false;
        
        PathManager.Instance.ChangePrefs(DecisionsHandler.Instance.badPath ? "BadPath" : "GoodPath");
        GameManagerNew.Instance.LoadSceneWithDelay("Hub",5);
        
    }
    
    IEnumerator HideEnemyLerp()
    {
        _corroutineActivate = true;
        foreach (var ps in smokePS)
        {
            ps.Stop();
        }
        while (enemyVisibility > 0)
        {
            enemyVisibility -= .3f;
            enemyMaterial.SetFloat("_Power", enemyVisibility);
            yield return new WaitForSeconds(0.1f);
        }
        enemyVisible = false;
        _corroutineActivate = false;
    }

    private void CompareRooms()
    {
        if (_player.actualRoom == null) return;
        if (_player.actualRoom != actualRoom)
        {
            if (objects.Count() > 0)
            {
                foreach (var obj in objects)
                {
                    obj.OnEndInteract();
                }
            }
            objects.Clear();
            canInteract = true;
            return;
        }

        if (!canInteract) return;
        canInteract = false;
        var objectsInRoom = actualRoom.interactableEnemy.ToList();
        for (int i = 0; i < 6; i++)
        {
            if (objectsInRoom.Count() == 0) break;
            int randomIndex = Random.Range(0, objectsInRoom.Count);
            var actualObject = objectsInRoom[randomIndex];
            objects.Add(actualObject.GetComponent<IInteractableEnemy>());
            objectsInRoom.Remove(actualObject);
        }

        foreach (var obj in objects)
        {
            obj.OnStartInteract();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 16) return;

        actualRoom = other.GetComponent<Room>();
    }

    public void CheckRoom()
    {
        if (actualRoom != PlayerHandler.Instance.actualRoom) return;
        crossUsed = true;
        _enemyAnimator.ChangeStateAnimation("CrossUsed", true);
    }

    public void RitualReady(Node node)
    {
        nodeRitual = node;
        ritualDone = true;
    }

    public Vector3 MoveSmooth(Vector3 target)
    {
        var dir = (target - transform.position).normalized;

        return dir;
    }

}
