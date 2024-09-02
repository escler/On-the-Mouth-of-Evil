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
        print("Asd");
        _idleTime = constTime + Random.Range(variantTimeMin, variantTimeMax);

    }

    public override IState ProcessInput()
    {
        if (_idleTime <= 0 && Transitions.ContainsKey(StateTransitions.ToPatrol))
            return Transitions[StateTransitions.ToPatrol];

        if (owner.bibleBurning && Transitions.ContainsKey(StateTransitions.ToSpecifyLocation))
            return Transitions[StateTransitions.ToSpecifyLocation];

        return this;
    }
}
