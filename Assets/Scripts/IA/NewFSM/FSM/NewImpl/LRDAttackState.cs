using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class LRDAttackState : MonoBaseState
{
    [SerializeField] float cd;
    [SerializeField] DemonLowRange owner;

    float timer;

    public override IState ProcessInput()
    {
        if (owner.EnemyBanished() && Transitions.ContainsKey(StateTransitions.ToBanish))
            return Transitions[StateTransitions.ToBanish];
        
        if(timer >= cd && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];

        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        Debug.Log("Ataco");
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        timer = 0;
        owner.CdForAttack = cd * 2;
        return base.Exit(to);
    }

    public override void UpdateLoop()
    {
        timer += Time.deltaTime;

    }
}
