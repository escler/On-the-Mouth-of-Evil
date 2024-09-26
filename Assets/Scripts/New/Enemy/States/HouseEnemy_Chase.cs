using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class HouseEnemy_Chase : MonoBaseState
{
    [SerializeField] private HouseEnemy owner;
    private bool _ray;
    private bool _headGrabbed;
    public List<Vector3> _path;
    public Node startNode, goal;
    public float timeToLostPlayer;
    private float _actualTime;
    private bool _playerLost;
    private float _actualGrabCD;
    public float grabCD;
    public override void UpdateLoop()
    {
        var playerPos = PlayerHandler.Instance.transform.position;
        var dir = playerPos - transform.position;
        if (_actualGrabCD > 0)
        {
            _actualGrabCD -= Time.deltaTime;
        }
        if (_actualTime <= 0)
            _playerLost = true;

        _ray = Physics.Raycast(transform.position, dir, dir.magnitude, owner.obstacles);

        if (!_ray)
        {
            MoveToPlayer();
            return;
        }
        
        CalculatePath();
        _actualTime -= Time.deltaTime;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        print("Entre a Chase");
        owner.chasePlayer = true;
        _actualTime = timeToLostPlayer;
        _actualGrabCD = grabCD/2;
    }
    
    public override Dictionary<string, object> Exit(IState to)
    {
        owner.actualTimeChase = owner.cdChase;
        _path.Clear();
        startNode = null;
        goal = null;
        owner.chasePlayer = false;
        _playerLost = false;
        print("Sali del chase");
        return base.Exit(to);
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
        Vector3 target = _path[0];
        target.y = owner.transform.position.y;
        Vector3 dir = target - owner.transform.position;
        owner.transform.rotation = Quaternion.LookRotation(dir);
        owner.transform.position += dir.normalized * (owner.speed / 4 * Time.deltaTime);
        
        if (Vector3.Distance(target, owner.transform.position) <= 0.1f || target == null) _path.RemoveAt(0);
    }

    public void MoveToPlayer()
    {
        _actualTime = timeToLostPlayer;

        Vector3 target = PlayerHandler.Instance.transform.position;
        target.y = owner.transform.position.y;
        if (Vector3.Distance(target, owner.transform.position) < .75) 
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
    }

    public override IState ProcessInput()
    {
        if (owner.ritualDone && Transitions.ContainsKey(StateTransitions.ToRitual))
            return Transitions[StateTransitions.ToRitual];
        
        if (_playerLost && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];
        
        if (owner.crossUsed && Transitions.ContainsKey(StateTransitions.ToPatrol))
            return Transitions[StateTransitions.ToPatrol];
        
        if (owner.bibleBurning && Transitions.ContainsKey(StateTransitions.ToSpecifyLocation))
            return Transitions[StateTransitions.ToSpecifyLocation];
        
        return this;
    }
}
