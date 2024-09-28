using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class HouseEnemy_Idle : MonoBaseState
{
    [SerializeField] private HouseEnemy owner;
    [SerializeField] private float constTime, variantTimeMin, variantTimeMax;
    private float _idleTime;
    
    public override void UpdateLoop()
    {
        _idleTime -= Time.deltaTime;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        _idleTime = constTime + Random.Range(variantTimeMin, variantTimeMax);
        print("Entre a Idle");
    }

    public override IState ProcessInput()
    {
        
        if (owner.ritualDone && Transitions.ContainsKey(StateTransitions.ToRitual))
            return Transitions[StateTransitions.ToRitual];
        
        if (_idleTime <= 0 && Transitions.ContainsKey(StateTransitions.ToPatrol))
            return Transitions[StateTransitions.ToPatrol];

        if (owner.crossUsed && Transitions.ContainsKey(StateTransitions.ToPatrol))
            return Transitions[StateTransitions.ToPatrol];

        if (owner.bibleBurning && Transitions.ContainsKey(StateTransitions.ToSpecifyLocation))
            return Transitions[StateTransitions.ToSpecifyLocation];

        if (owner.canAttackPlayer && Transitions.ContainsKey(StateTransitions.ToChase)) 
            return Transitions[StateTransitions.ToChase];

        return this;
    }
}
