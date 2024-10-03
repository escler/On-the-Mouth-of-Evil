using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class HouseEnemy_Attacks : MonoBaseState
{
    [SerializeField] private HouseEnemy owner;
    private bool _ray;
    private bool _headGrabbed;
    public List<Transform> _path;
    public Node startNode, goal;
    public float timeToLostPlayer;
    private float _actualTime;
    private float _actualGrabCD;
    public float grabCD;
    public int[] enemyAction = { 0, 1, 2 }; //0 - Chase and Grab Head / 1 - Cordura Attack / 2 - Block Doors 
    private int _actualAction;
    private bool animationStarted;
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
        print("Entre a Chase");
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

        owner.attackEnded = false;

        return base.Exit(to);
    }


    #region Chase and GrabHead

    void OnEnterChase()
    {
        owner.attackEnded = false;
        owner.actualTimeToLost = timeToLostPlayer;
    }
    private void OnUpdateChase()
    {
        var playerPos = PlayerHandler.Instance.transform.position;
        var dir = playerPos - transform.position;

        if (owner.actualTimeToLost <= 0) owner.attackEnded = true;
        
        if (_actualGrabCD > 0)
        {
            _actualGrabCD -= Time.deltaTime;
        }

        _ray = Physics.Raycast(transform.position, dir, dir.magnitude, owner.obstacles);

        if (!_ray)
        {
            MoveToPlayer();
            owner.actualTimeToLost = timeToLostPlayer;
            return;
        }

        
        CalculatePath();
    }

    private void OnExitChase()
    {
        _path.Clear();
        startNode = null;
        goal = null;
        owner.grabHead = false;
    }
    private void CalculatePath()
    {
        if(_path.Count > 0)
            TravelPath();
        _path.Clear();
        startNode = PathFindingManager.instance.CalculateDistance(owner.transform.position);
        goal = PathFindingManager.instance.CalculateDistance(PlayerHandler.Instance.transform.position);
        
        _path = owner.pf.ThetaStar(startNode,goal, owner.obstacles);

        if (_path.Count > 0)
        {
            _path.Reverse();
            TravelPath();
        }
    }
    
    private void TravelPath()
    {
        Vector3 target = _path[0].position;
        target.y = owner.transform.position.y;
        Vector3 dir = target - owner.transform.position;
        owner.transform.rotation = Quaternion.LookRotation(dir);
        owner.transform.position += dir.normalized * (owner.speed / 4 * Time.deltaTime);
        
        if (Vector3.Distance(target, owner.transform.position) <= 0.1f || target == null) _path.RemoveAt(0);
    }

    public void MoveToPlayer()
    {

        Vector3 target = PlayerHandler.Instance.transform.position;
        target.y = owner.transform.position.y;
        if (Vector3.Distance(target, owner.transform.position) < .75f) 
        {
            if(_actualGrabCD <= 0)GrabHead();
            return;
        }
        
        _headGrabbed = false;
        Vector3 dir = target - owner.transform.position;
        owner.transform.rotation = Quaternion.LookRotation(dir);
        owner.transform.position += dir.normalized * (owner.speed / 2 * Time.deltaTime);

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
        owner.EnemyAnimator.ChangeStateAnimation("CorduraAttack", true);
        if (CorduraHandler.Instance.CorduraOn > 0)
        {
            owner.attackEnded = true;
            print("No activo cor");
            return;
        }
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
            return Transitions[StateTransitions.ToPatrol];
        
        if (owner.attackEnded && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];
        
        if (!owner.enemyVisible && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];
        
        if (owner.bibleBurning && Transitions.ContainsKey(StateTransitions.ToSpecifyLocation))
            return Transitions[StateTransitions.ToSpecifyLocation];
        
        return this;
    }
}
