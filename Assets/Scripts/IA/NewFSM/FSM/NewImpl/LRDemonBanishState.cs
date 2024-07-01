using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class LRDemonBanishState : MonoBaseState
{
    [SerializeField] DemonLowRange owner;
    [SerializeField] private float timeToBanish;
    public override IState ProcessInput()
    {
        if (timeToBanish < 0 && Transitions.ContainsKey(StateTransitions.ToDeath) || owner.banished)
            return Transitions[StateTransitions.ToDeath];
        return this;
    }
    
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from,transitionParameters);
        owner.animator.SetParameter("Death", true);
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        owner.banished = true;
        owner.canBanish = false;
        owner.GetComponentInChildren<CapsuleCollider>().enabled = false;
        return base.Exit(to);
    }

    public override void UpdateLoop()
    {
        if (!owner.onBanishing) timeToBanish -= Time.deltaTime;
    }
}
