using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class MorgueLevel_ToLocation : MonoBaseState
{
    [SerializeField] private MorgueEnemy owner;
    private List<Transform> _path;
    private Node _startNode, _goalNode;
    private bool _pathCalculated, _pathFinish, bibleBurning;
    
    
    public override void UpdateLoop()
    {
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        print("Entre a GoToLocation");
        _startNode = PathFindingManager.instance.CalculateDistance(owner.transform.position);
        _goalNode = owner.ritualDone ? owner.nodeRitual : PathFindingManager.instance.CalculateDistance(owner.goalPosition);


        
        if (_goalNode == null) _pathFinish = true;
        else GoToNode();
    }

    private void GoToNode()
    {
        StartCoroutine(MoveToLocation());
    }

    IEnumerator MoveToLocation()
    {
        yield return new WaitForSeconds(.5f);
        Vector3 goalPos = owner.goalPosition;
        goalPos.y = owner.transform.position.y;
        transform.position = goalPos;
        _pathFinish = true;
    }
    
    

    public override Dictionary<string, object> Exit(IState to)
    {
        StopCoroutine(MoveToLocation());
        _pathFinish = false;
        return base.Exit(to);
    }

    public override IState ProcessInput()
    {
        if (owner.ritualDone && Transitions.ContainsKey(StateTransitions.ToRitual))
            return Transitions[StateTransitions.ToRitual];

        if (owner.crossUsed && !owner.ritualDone && Transitions.ContainsKey(StateTransitions.ToPatrol))
            return Transitions[StateTransitions.ToPatrol];

        if (!owner.bibleBurning && _pathFinish && !owner.ritualDone && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];
        
        if (!owner.ritualDone && _pathFinish && Transitions.ContainsKey(StateTransitions.ToSpecifyLocation))
            return Transitions[StateTransitions.ToSpecifyLocation];
        
        return this;
    }
}
