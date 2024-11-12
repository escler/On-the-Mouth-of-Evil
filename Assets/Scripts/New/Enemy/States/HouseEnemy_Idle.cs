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
        if(!owner.canAttackPlayer) owner.HideEnemy();
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
        
        if (owner.voodooActivate && Transitions.ContainsKey(StateTransitions.ToVoodoo))
            return Transitions[StateTransitions.ToVoodoo];

        if (owner.actualTime > owner.timeToShowMe && !owner.ritualDone &&
            Transitions.ContainsKey(StateTransitions.ToSpawn))
            return Transitions[StateTransitions.ToSpawn];
        
        if (_idleTime <= 0 && Transitions.ContainsKey(StateTransitions.ToPatrol))
            return Transitions[StateTransitions.ToPatrol];

        if (owner.crossUsed && Transitions.ContainsKey(StateTransitions.ToPatrol))
            return Transitions[StateTransitions.ToPatrol];

        if (owner.bibleBurning && Transitions.ContainsKey(StateTransitions.ToSpecifyLocation))
            return Transitions[StateTransitions.ToSpecifyLocation];

        if (owner.canAttackPlayer && Transitions.ContainsKey(StateTransitions.ToAttacks)) 
            return Transitions[StateTransitions.ToAttacks];

        return this;
    }
}
