using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;
using Random = UnityEngine.Random;

public class HouseEnemy_Attacks : MonoBaseState
{
    [SerializeField] private HouseEnemy owner;
    private bool _ray;
    private bool _headGrabbed;
    public List<Transform> _path;
    public Node startNode, goal;
    public float timeToLostPlayer;
    private float _actualTime;
    public float hipnosisTime;
    private float _actualGrabCD;
    public float grabCD;
    public int[] enemyAction = { 0, 1, 2 }; //0 - Chase and Grab Head / 1 - Cordura Attack / 2 - Block Doors 
    private int _actualAction;
    private bool animationStarted;
    private float waitingTime;
    private bool _corroutine;
    public override void UpdateLoop()
    {
        switch (_actualAction)
        {
            case 0:
                OnUpdateChase();
                break;
            case 1:
                OnUpdateCorduraAttack();
                break;
            case 2:
                OnUpdateBlockDoorAttack();
                break;
        }
    }


    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        print("Entre a Attacks");
        _actualAction = owner.compareRoom ? Random.Range(0, enemyAction.Length) : 0;
        switch (_actualAction)
        {
            case 0:
                OnEnterChase();
                break;
            case 1:
                OnEnterCorduraAttack();
                break;
            case 2:
                OnEnterBlockDoorAttack();
                break;
        }
    }
    
    public override Dictionary<string, object> Exit(IState to)
    {
        switch (_actualAction)
        {
            case 0:
                OnExitChase();
                break;
            case 1:
                OnExitCorduraAttack();
                break;
            case 2:
                OnExitBlockDoorAttack();
                break;
        }
        print("Sali de attack");
        StopCoroutine(TeleportCor());
        waitingTime = 0;

        owner.attackEnded = false;

        return base.Exit(to);
    }


    #region Chase and GrabHead

    void OnEnterChase()
    {
        owner.attackEnded = false;
        Teleport();
    }
    private void OnUpdateChase()
    {
        var playerPos = PlayerHandler.Instance.transform.position;
        playerPos.y = owner.transform.position.y;
        var dir = playerPos - transform.position;

        transform.LookAt(Vector3.SmoothDamp(transform.position,playerPos, ref owner.reference, owner.rotationSmoothTime), Vector3.up);

        if (owner.actualTimeToLost <= 0)
        {
            owner.attackEnded = true;
            return;
        }
        
        if (_actualGrabCD > 0)
        {
            _actualGrabCD -= Time.deltaTime;
        }
        _ray = Physics.Raycast(transform.position, dir, dir.magnitude, owner.obstacles);
        
        if (_ray) return;
        
        owner.actualTimeToLost = timeToLostPlayer;
    }

    private void OnExitChase()
    {
        _path.Clear();
        startNode = null;
        goal = null;
        StopCoroutine(Hipnosis());
        StopCoroutine(WaitAnimState());
        PlayerHandler.Instance.PossesPlayer();
        owner.grabHead = false;
        _corroutine = false;
        _headGrabbed = false;
    }
    private void Teleport()
    {
        if (_corroutine || _headGrabbed) return;
        if (waitingTime > 0)
        {
            waitingTime -= Time.deltaTime;
            return;
        }
        var playerPos = PlayerHandler.Instance.transform.position;
        playerPos.y = owner.transform.position.y;
        if (goal == null)
        {
            startNode = PathFindingManager.instance.CalculateDistance(owner.transform.position);
            goal = PathFindingManager.instance.CalculateNearnestNodeAndRoom(playerPos);
        }

        if (startNode == goal && _actualGrabCD <= 0)
        {
            StartCoroutine(Hipnosis());
            return;
        }

        StartCoroutine(TeleportCor());
    }

    IEnumerator TeleportCor()
    {
        _corroutine = true;
        TriggerDissapear();
        while (owner.enemyVisibility > 0)
        {
            owner.enemyVisibility -= .5f;
            owner.enemyMaterial.SetFloat("_Power", owner.enemyVisibility);
            yield return new WaitForSeconds(0.1f);
        }

        if (goal != null)
        {
            TriggerAppear();
            Vector3 target = goal.transform.position;
            target.y = owner.transform.position.y;
            owner.transform.position = target;
        }

        while (owner.enemyVisibility < 8)
        {
            owner.enemyVisibility += .5f;
            owner.enemyMaterial.SetFloat("_Power", owner.enemyVisibility);
            yield return new WaitForSeconds(0.1f);
        }

        waitingTime = 4f;
        goal = null;
        _corroutine = false;
        
        if(_actualGrabCD <= 0) StartCoroutine(Hipnosis());
    }
    
    private void TriggerAppear()
    {
        owner.EnemyAnimator.animator.SetTrigger("Appear");
        owner.EnemyAnimator.PSAppear.SetActive(true);
    }

    private void TriggerDissapear()
    {
        owner.EnemyAnimator.animator.SetTrigger("Dissapear");
    }

    private IEnumerator Hipnosis()
    {
        if (_ray)
        {
            owner.attackEnded = true;
            yield break;
        }
        _corroutine = true;
        float time = hipnosisTime;
        Transform player = PlayerHandler.Instance.transform;
        while (time > 0 && !_ray)
        {
            if (owner.crossUsed) break;
            Vector3 target = owner.transform.position;
            target.y = player.position.y;
            time -= 0.1f;
            PlayerHandler.Instance.UnPossesPlayer();
            player.LookAt(Vector3.SmoothDamp(transform.position, target, ref owner.reference, owner.rotationSmoothTime),
                Vector3.up);
            player.GetComponent<Rigidbody>().velocity = -transform.forward * 50 * Time.fixedDeltaTime;
            if (Vector3.Distance(target, player.transform.position) < .7)
            {
                GrabHead();
                StopCoroutine(Hipnosis());
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        
        PlayerHandler.Instance.PossesPlayer();
        _corroutine = false;
        owner.attackEnded = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(owner.obstaclePosRight.position, transform.forward);
        Gizmos.DrawRay(owner.obstaclePosLeft.position, transform.forward);

    }

    private void GrabHead()
    {
        if (_headGrabbed) return;
        _headGrabbed = true;
        StartCoroutine(WaitAnimState());
    }
    
    IEnumerator WaitAnimState()
    {
        owner.EnemyAnimator.ActivateGrabHead();
        yield return new WaitUntil(() =>
            owner.EnemyAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName("Grabhead"));
        
        owner.grabHead = true;
        PlayerHandler.Instance.HeadGrabbed(owner.transform);
        while (owner.EnemyAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName("Grabhead"))
        {
            Quaternion lookDirection = Quaternion.LookRotation(PlayerHandler.Instance.transform.position - owner.transform.position).normalized;
            lookDirection.x = transform.rotation.x;
            lookDirection.z = transform.rotation.z;

            owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, lookDirection, 10f * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }

        owner.playerGrabbedCount++;
        PlayerLifeHandlerNew.Instance.DamageTaked(1);
        if (owner.playerGrabbedCount > 2) GameManagerNew.Instance.LoadSceneWithDelay("Hub", 0.1f);

        _actualGrabCD = grabCD;
        owner.grabHead = false;
        _headGrabbed = false;
        owner.attackEnded = true;
    }

    #endregion

    #region CorduraAttack

    void OnEnterCorduraAttack()
    {
        owner.attackEnded = false;
        if(owner.compareRoom) owner.actualTimeToLost = timeToLostPlayer;
        if (!owner.compareRoom && owner.actualTimeToLost > 0)
        {
            _actualAction = 0;
            OnEnterChase();
            return;
        }
        if (CorduraHandler.Instance.CorduraOn > 0)
        {
            owner.attackEnded = true;
            print("No activo cor");
            return;
        }
        owner.EnemyAnimator.ChangeStateAnimation("CorduraAttack", true);
        StartCoroutine(CorduraAttackCor());
    }

    IEnumerator CorduraAttackCor()
    {
        yield return new WaitUntil(() => owner.EnemyAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName("Point"));
        yield return new WaitUntil(() => !owner.EnemyAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName("Point"));
        owner.attackEnded = true;
    }

    void OnUpdateCorduraAttack()
    {
        var playerPos = PlayerHandler.Instance.transform.position;
        var dir = playerPos - transform.position;
        
        _ray = Physics.Raycast(transform.position, dir, dir.magnitude, owner.obstacles);

        if (!_ray)
        {
            owner.actualTimeToLost = timeToLostPlayer;
        }
        
        Quaternion lookDirection = Quaternion.LookRotation(PlayerHandler.Instance.transform.position - owner.transform.position).normalized;
        lookDirection.x = transform.rotation.x;
        lookDirection.z = transform.rotation.z;

        owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, lookDirection, 10f * Time.deltaTime);
    }

    void OnExitCorduraAttack()
    {
    }

    #endregion

    #region BloockDoorAttack

    private void OnEnterBlockDoorAttack()
    {
        print("entre a block");
        if (owner.compareRoom) owner.actualTimeToLost = timeToLostPlayer;
        if (!owner.actualRoom.DoorsBlocked() || owner.actualRoom.movableItems.Length <= 0)
        {
            OnEnterChase();
            _actualAction = 0;
            return;
        }
        owner.attackEnded = false;
        owner.EnemyAnimator.ChangeStateAnimation("BlockDoor", true);
        StartCoroutine(WaitForAnimationEnd());
    }

    IEnumerator WaitForAnimationEnd()
    {
        yield return new WaitUntil(() => owner.EnemyAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));
        yield return new WaitUntil(() => !owner.EnemyAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));
        owner.attackEnded = true;
    }

    private void OnUpdateBlockDoorAttack()
    {
        var playerPos = PlayerHandler.Instance.transform.position;
        var dir = playerPos - transform.position;
        
        _ray = Physics.Raycast(transform.position, dir, dir.magnitude, owner.obstacles);

        if (!_ray)
        {
            owner.actualTimeToLost = timeToLostPlayer;
        }
    }
    private void OnExitBlockDoorAttack(){}

    #endregion

    public override IState ProcessInput()
    {
        if (owner.ritualDone && Transitions.ContainsKey(StateTransitions.ToRitual))
            return Transitions[StateTransitions.ToRitual];

        if (owner.crossUsed && Transitions.ContainsKey(StateTransitions.ToPatrol))
        {
            return Transitions[StateTransitions.ToPatrol];
        }
        
        if (owner.attackEnded && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];
        
        if (!owner.enemyVisible && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];
        
        if (owner.bibleBurning && Transitions.ContainsKey(StateTransitions.ToSpecifyLocation))
            return Transitions[StateTransitions.ToSpecifyLocation];
        
        return this;
    }
}
