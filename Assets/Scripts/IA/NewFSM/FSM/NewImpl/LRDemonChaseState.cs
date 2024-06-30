using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class LRDemonChaseState : MonoBaseState
{
    [SerializeField] DemonLowRange owner;

    [SerializeField] float speed = 5;
    [SerializeField] private float intensity;

    public override IState ProcessInput()
    {
        if (owner.canBanish && Transitions.ContainsKey(StateTransitions.ToBanish))
            return Transitions[StateTransitions.ToBanish];
        
        if (owner.IsAttackDistance() && owner.CanAttack() && Transitions.ContainsKey(StateTransitions.ToAttack))
            return Transitions[StateTransitions.ToAttack];

        if (owner.IsAttackDistance() && !owner.CanAttack() && Transitions.ContainsKey(StateTransitions.ToMoveAround))
            return Transitions[StateTransitions.ToMoveAround];

        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        owner.animator.SetParameter("Run", true);
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        owner.animator.SetParameter("Run", false);
        return base.Exit(to);
    }

    public override void UpdateLoop()
    {
        var dir = (owner.target.position - owner.transform.position).normalized;
        var ray1 = Physics.Raycast(owner.transform.position + transform.right * .5f, transform.forward,
            Vector3.Distance(owner.target.position, owner.transform.position), owner.layer);
        var ray2 = Physics.Raycast(owner.transform.position - transform.right * .5f, transform.forward,
            Vector3.Distance(owner.target.position, owner.transform.position), owner.layer);

        if (ray1) dir -= transform.right * intensity * Time.deltaTime;
        else if (ray2) dir += transform.right * intensity * Time.deltaTime;
        owner.transform.LookAt(owner.target.position + dir);

        owner.transform.position += dir * Time.deltaTime * speed;
        owner.EntityMove();

    }
}
