using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class LRDemonChaseState : MonoBaseState
{
    [SerializeField] DemonLowRange owner;

    [SerializeField] float speed = 5;

    public override IState ProcessInput()
    {
        if (owner.EnemyBanished() && Transitions.ContainsKey(StateTransitions.ToBanish))
            return Transitions[StateTransitions.ToBanish];
        
        if (owner.IsAttackDistance() && owner.CanAttack() && Transitions.ContainsKey(StateTransitions.ToAttack))
            return Transitions[StateTransitions.ToAttack];

        if (owner.IsAttackDistance() && !owner.CanAttack() && Transitions.ContainsKey(StateTransitions.ToMoveAround))
            return Transitions[StateTransitions.ToMoveAround];

        return this;
    }

    public override void UpdateLoop()
    {
        var dir = (owner.target.position - owner.transform.position).normalized;

        owner.transform.position += dir * Time.deltaTime * speed;
    }
}
