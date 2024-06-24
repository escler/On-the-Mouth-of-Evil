using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class LRDemonIdleState : MonoBaseState
{
    [SerializeField] DemonLowRange owner;

    public override IState ProcessInput()
    {
        if (owner.EnemyBanished() && Transitions.ContainsKey(StateTransitions.ToBanish))
            return Transitions[StateTransitions.ToBanish];
        
        if(owner.IsAttackDistance() && owner.CanAttack() && Transitions.ContainsKey(StateTransitions.ToAttack))
            return Transitions[StateTransitions.ToAttack];

        if(owner.IsPersuitDistance() && Transitions.ContainsKey(StateTransitions.ToChase))
            return Transitions[StateTransitions.ToChase];

        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        Debug.Log("Estoy en idle");
    }

    public override void UpdateLoop()
    {
        
    }
}
