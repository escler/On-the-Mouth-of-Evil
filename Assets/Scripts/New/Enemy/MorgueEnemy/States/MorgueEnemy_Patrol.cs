using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class MorgueEnemy_Patrol : MonoBaseState
{
    [SerializeField] private MorgueEnemy owner;
    public List<Transform> _path;
    public Node startNode, goal;
    private bool _pathCalculated, _pathFinish;
    public override void UpdateLoop()
    {

    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        _path.Clear();
        print("Entre a Patrol");
        
        startNode = PathFindingManager.instance.CalculateDistance(owner.transform.position);
        goal = PathFindingManager.instance.CalculateOtherRoomNodeMorgue(startNode);
        if (owner.crossUsed)
        {
            print("Entre al if");
            CrossUsed();
            return;
        }

        if (goal == null)
        {
            print("Goal null");
            _pathFinish = true;
        }
        else GoToNode();
    }

    private void CrossUsed()
    {
        print("Entre al metodo");
        StartCoroutine(HideEnemy());
    }

    
    IEnumerator HideEnemy()
    {
        
        while (owner.enemyVisibility > 0)
        {
            owner.enemyVisibility -= .3f;
            owner.enemyMaterial.SetFloat("_Power", owner.enemyVisibility);
            yield return new WaitForSeconds(0.1f);
        }
        owner.enemyVisible = false;
        
        yield return null;
        owner.crossUsed = false;
        owner.actualTime = 0;
        if (goal == null)
        {
            print("Goal null");
            _pathFinish = true;
        }
        else GoToNode();
    }
    private void GoToNode()
    {
        Vector3 goalPos = goal.transform.position;
        goalPos.y = owner.transform.position.y;
        owner.transform.position = goalPos;
        _pathFinish = true;
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
        
        if (owner.voodooActivate && Transitions.ContainsKey(StateTransitions.ToVoodoo))
            return Transitions[StateTransitions.ToVoodoo];
        
        
        if (owner.bibleBurning && Transitions.ContainsKey(StateTransitions.ToSpecifyLocation))
            return Transitions[StateTransitions.ToSpecifyLocation];
        
        if (owner.crossUsed && Transitions.ContainsKey(StateTransitions.ToPatrol))
            return Transitions[StateTransitions.ToPatrol];

        return this;
    }
}
