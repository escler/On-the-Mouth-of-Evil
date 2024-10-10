using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class HouseEnemy_Patrol : MonoBaseState
{
    [SerializeField] private HouseEnemy owner;
    public List<Transform> _path;
    public Node startNode, goal;
    private bool _pathCalculated, _pathFinish;
    public override void UpdateLoop()
    {
        if (!_pathCalculated) return;

        if (_path.Count > 0)
        {
            TravelPath();
            return;
        }
        
        GoToNodeGoal();
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        _path.Clear();
        print("Entre a Patrol");

        
        startNode = PathFindingManager.instance.CalculateDistance(owner.transform.position);
        goal = PathFindingManager.instance.CalculateOtherRoomNode(startNode);
        
        _path = owner.pf.ThetaStar(startNode,goal, owner.obstacles);

        if (_path.Count > 0)
        {
            _path.Reverse();
            _pathCalculated = true;
            if (owner.crossUsed)
            {
                owner.actualTimeToLost = 0;
                owner.crossUsed = false;
            }
        }
        else
        {
            print("No tengo path");
            _pathFinish = true;
        }
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        _path.Clear();
        _pathCalculated = false;
        _pathFinish = false;
        startNode = null;
        goal = null;
        return base.Exit(to);
    }

    public override IState ProcessInput()
    {
        if (owner.ritualDone && Transitions.ContainsKey(StateTransitions.ToRitual))
            return Transitions[StateTransitions.ToRitual];
        
        if (_pathFinish && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];

        if (owner.bibleBurning && Transitions.ContainsKey(StateTransitions.ToSpecifyLocation))
            return Transitions[StateTransitions.ToSpecifyLocation];

        if (owner.crossUsed && Transitions.ContainsKey(StateTransitions.ToPatrol))
            return Transitions[StateTransitions.ToPatrol];
        return this;
    }
    
    private void TravelPath()
    {
        if (_path[0].GetComponent<Node>().blocked) _pathFinish = true;
        Vector3 target = _path[0].position;
        target.y = transform.position.y;
        var dir = owner.MoveSmooth(target);
        var ray1 = Physics.Raycast(owner.obstaclePosRight.position, owner.obstaclePosRight.forward, owner.distanceObstacleRay,owner.obstacles);
        var ray2 = Physics.Raycast(owner.obstaclePosLeft.position, owner.obstaclePosLeft.forward,
            owner.distanceObstacleRay, owner.obstacles);

        if (ray1) dir -= transform.right;
        else if (ray2) dir += transform.right;
        
        transform.LookAt(Vector3.SmoothDamp(transform.position, target + dir, ref owner.reference, owner.rotationSmoothTime / 2), Vector3.up);
        transform.position += dir * Time.deltaTime * owner.speed * 2;
        if (Vector3.Distance(target, owner.transform.position) <= 0.3f || target == null)
        {
            print("Entre");
            _path.RemoveAt(0);
        }
    }
    
    private void GoToNodeGoal()
    {
        if (_pathFinish) return;
        Vector3 target = goal.transform.position;
        target.y = owner.transform.position.y;
        var dir = owner.MoveSmooth(target);
        
        var ray1 = Physics.Raycast(owner.obstaclePosRight.position, owner.obstaclePosRight.forward, owner.distanceObstacleRay,owner.obstacles);
        var ray2 = Physics.Raycast(owner.obstaclePosLeft.position, owner.obstaclePosLeft.forward,
            owner.distanceObstacleRay, owner.obstacles);

        if (ray1) dir -= transform.right * owner.intensityObstacleAvoidance * Time.deltaTime;
        else if (ray2) dir += transform.right * owner.intensityObstacleAvoidance * Time.deltaTime;
        
        transform.LookAt(Vector3.SmoothDamp(transform.position, target + dir, ref owner.reference, owner.rotationSmoothTime / 2), Vector3.up);
        transform.position += dir * Time.deltaTime * owner.speed * 2;
        
        if (Vector3.Distance(target, owner.transform.position) <= 0.1f) _pathFinish = true;
    }

}
