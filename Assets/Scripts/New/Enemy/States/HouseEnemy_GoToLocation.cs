using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class HouseEnemy_GoToLocation : MonoBaseState
{
    [SerializeField] private HouseEnemy owner;
    private List<Vector3> _path;
    private Node _startNode, _goalNode;
    private bool _pathCalculated, _pathFinish, bibleBurning;
    
    
    public override void UpdateLoop()
    {
        if (!_pathCalculated) return;

        if(_path.Count > 0)
            TravelPath();
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        _startNode = PathFindingManager.instance.CalculateDistance(owner.transform.position);
        _goalNode = PathFindingManager.instance.CalculateDistance(owner.goalPosition);
        _path = owner.pf.ThetaStar(_startNode, _goalNode, owner.obstacles);

        if (_path.Count > 0)
        {
            _path.Reverse();
            _pathCalculated = true;
        }
    }
    
    private void TravelPath()
    {
        Vector3 target = _path[0];
        target.y = owner.transform.position.y;
        Vector3 dir = target - owner.transform.position;
        owner.transform.rotation = Quaternion.LookRotation(dir);
        owner.transform.position += dir.normalized * (owner.speed * Time.deltaTime);
        
        if (Vector3.Distance(target, owner.transform.position) <= 0.1f) _path.RemoveAt(0);
        if (_path.Count == 0) _pathFinish = true;
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        _path.Clear();
        _pathCalculated = false;
        _pathFinish = false;
        return base.Exit(to);
    }

    public override IState ProcessInput()
    {
        if (!owner.bibleBurning && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];
        
        return this;
    }
}
