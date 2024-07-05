using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class LRDemonIdleState : MonoBaseState
{
    [SerializeField] DemonLowRange owner;

    private float _timeToTransition;
    public override IState ProcessInput()
    {
        if (owner.canBanish && Transitions.ContainsKey(StateTransitions.ToBanish))
            return Transitions[StateTransitions.ToBanish];

        if (owner.ReactHit() && Transitions.ContainsKey(StateTransitions.ToHit))
            return Transitions[StateTransitions.ToHit];
        
        if(_timeToTransition < 0 && owner.IsAttackDistance() && owner.CanAttack() && Transitions.ContainsKey(StateTransitions.ToAttack))
            return Transitions[StateTransitions.ToAttack];

        if(_timeToTransition < 0 && owner.IsPersuitDistance() && owner.IsAttackDistance() && Transitions.ContainsKey(StateTransitions.ToChase))
            return Transitions[StateTransitions.ToChase];
        
        if(_timeToTransition < 0 && !owner.IsAttackDistance() && Transitions.ContainsKey(StateTransitions.ToMoveAround))
            return Transitions[StateTransitions.ToMoveAround];

        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        owner.animator.SetParameter("MoveAround", false);
        _timeToTransition = Random.Range(.6f, 1f);
    }

    public override void UpdateLoop()
    {
        _timeToTransition -= Time.deltaTime;
    }
}
