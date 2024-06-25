using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class LRDDemonMoveAroundState : MonoBaseState
{
    [SerializeField] DemonLowRange owner;
    private float _timeToTransition;
    [SerializeField] private float speed;
    private int _direction;
    public override IState ProcessInput()
    {
        if (owner.canBanish && Transitions.ContainsKey(StateTransitions.ToBanish))
            return Transitions[StateTransitions.ToBanish];

        if (_timeToTransition < 0 && owner.IsAttackDistance() && owner.CanAttack() && Transitions.ContainsKey(StateTransitions.ToAttack))
            return Transitions[StateTransitions.ToAttack];
        
        if (_timeToTransition < 0 && !owner.IsAttackDistance() && owner.CanAttack() && Transitions.ContainsKey(StateTransitions.ToChase))
            return Transitions[StateTransitions.ToChase];

        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        print("Entre a MoveAround");
        _timeToTransition = Random.Range(1, 3);
        _direction = RandomDir();
        owner.animator.SetParameter("MoveAround", true);
        owner.animator.Animator.SetFloat("xAxis", _direction);
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        owner.animator.SetParameter("MoveAround", false);
        return base.Exit(to);
    }

    public override void UpdateLoop()
    {
        owner.transform.LookAt(owner.target);
        owner.transform.position += transform.right * Time.deltaTime * speed * _direction;
        _timeToTransition -= Time.deltaTime;
        
        var ray1 = Physics.Raycast(owner.transform.position, -transform.right,
            1f, owner.layer);
        var ray2 = Physics.Raycast(owner.transform.position, transform.right,
            1f, owner.layer);

        if (ray1) _direction = 1;
        else if (ray2) _direction = -1;
        owner.EntityMove();
    }

    private int RandomDir()
    {
        var dir = Random.Range(-1, 2);
        if (dir == 0) dir = 1;

        return dir;
    }

}
