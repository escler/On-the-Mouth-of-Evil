using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class HouseEnemy_Patrol : MonoBaseState
{
    [SerializeField] private HouseEnemy owner;
    public List<Vector3> _path;
    public Node startNode, goal;
    private bool _pathCalculated, _pathFinish;
    public override void UpdateLoop()
    {
        if (!_pathCalculated) return;
        
        TravelPath();
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        print("Entre a Patrol");
        
        startNode = PathFindingManager.instance.CalculateDistance(owner.transform.position);
        print(startNode);
        goal = PathFindingManager.instance.CalculateOtherRoomNode(startNode);
        
        _path = owner.pf.ThetaStar(startNode,goal, owner.obstacles);

        if (_path.Count > 0)
        {
            _path.Reverse();
            _pathCalculated = true;
        }
        else
        {
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
        if (_pathFinish && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];

        if (owner.bibleBurning && Transitions.ContainsKey(StateTransitions.ToSpecifyLocation))
            return Transitions[StateTransitions.ToSpecifyLocation];

        return this;
    }
    
    private void TravelPath()
    {
        Vector3 target = _path[0];
        target.y = owner.transform.position.y;
        Vector3 dir = target - owner.transform.position;
        owner.transform.rotation = Quaternion.LookRotation(dir);
        owner.transform.position += dir.normalized * (owner.speed * Time.deltaTime);
        
        if (Vector3.Distance(target, owner.transform.position) <= 0.1f || target == null) _path.RemoveAt(0);
        if (_path.Count == 0) _pathFinish = true;
    }
}
