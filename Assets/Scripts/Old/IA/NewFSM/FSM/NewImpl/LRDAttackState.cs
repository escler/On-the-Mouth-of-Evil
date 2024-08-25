using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class LRDAttackState : MonoBaseState
{
    [SerializeField] float cd;
    [SerializeField] DemonLowRange owner;
    private bool attackFinish;

    float timer;

    public override IState ProcessInput()
    {
        if (owner.canBanish && Transitions.ContainsKey(StateTransitions.ToBanish))
            return Transitions[StateTransitions.ToBanish];
        
        if (owner.ReactHit() && Transitions.ContainsKey(StateTransitions.ToHit))
            return Transitions[StateTransitions.ToHit];
        
        if(attackFinish && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];

        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        owner.animator.SetParameter("Attack", true);
        owner.transform.LookAt(owner.target);
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        attackFinish = false;
        owner.cdForAttack = 5;
        owner.animator.SetParameter("Attack", false);
        return base.Exit(to);
    }

    public override void UpdateLoop()
    {
        if (owner.animator.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
            owner.animator.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .8f)
            attackFinish = true;
    }
}
