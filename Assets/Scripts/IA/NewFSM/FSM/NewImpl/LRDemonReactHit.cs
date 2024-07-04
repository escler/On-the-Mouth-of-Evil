using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class LRDemonReactHit : MonoBaseState
{
    [SerializeField] private DemonLowRange owner;
    [SerializeField] private float force;
    private Vector3 _direction;
    private bool _stateFinish;
    
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        owner.cantHit = true;
        owner.animator.SetParameter("Dir", owner.Dot);
        owner.animator.SetParameter("HitReact", true);
        _direction = (owner.target.position - owner.transform.position).normalized * -1;

    }

    public override Dictionary<string, object> Exit(IState to)
    {
        owner.cantHit = false;
        owner.ActualHit = 0;
        owner.animator.SetParameter("HitReact", false);
        _stateFinish = false;
        owner.CantChangeDirection = false;
        return base.Exit(to);
    }

    public override void UpdateLoop()
    {
        if (owner.animator.Animator.GetCurrentAnimatorStateInfo(0).IsName("React") &&
            owner.animator.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .8f) 
            _stateFinish = true;
        
        var ray = Physics.Raycast(owner.transform.position, _direction, 3f, owner.layer);

        if (!ray) owner.transform.position += _direction * force * Time.deltaTime;
    }

    public override IState ProcessInput()
    {
        if (_stateFinish && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];

        return this;
    }
}
