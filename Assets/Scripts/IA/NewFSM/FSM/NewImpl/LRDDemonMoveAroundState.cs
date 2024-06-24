using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class LRDDemonMoveAroundState : MonoBaseState
{
    [SerializeField] DemonLowRange owner;
    private float _timeToTransition;
    [SerializeField] private float speed;
    public override IState ProcessInput()
    {
        if (owner.EnemyBanished() && Transitions.ContainsKey(StateTransitions.ToBanish))
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
    }

    public override void UpdateLoop()
    {
        owner.transform.LookAt(owner.target);
        owner.transform.position += transform.right * Time.deltaTime * speed;
        _timeToTransition -= Time.deltaTime;

    }

}
