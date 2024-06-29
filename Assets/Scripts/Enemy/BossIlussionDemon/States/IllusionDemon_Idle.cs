using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class IllusionDemon_Idle : MonoBaseState
{
    [SerializeField] private IllusionDemon owner;
    private float _actualTimer;
    private readonly float _timer = 1f;

    public override IState ProcessInput()
    {
        if (_actualTimer <= 0 && Transitions.ContainsKey(StateTransitions.ToMoveAround))
            return Transitions[StateTransitions.ToMoveAround];

        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        _actualTimer = _timer;

    }

    public override void UpdateLoop()
    {
        if (_actualTimer > 0) _actualTimer -= Time.deltaTime;
    }
}
