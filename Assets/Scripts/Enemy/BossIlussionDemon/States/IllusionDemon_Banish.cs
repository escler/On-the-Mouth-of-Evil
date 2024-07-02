using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class IllusionDemon_Banish : MonoBaseState
{
    [SerializeField] IllusionDemon owner;
    private float _actualTimer;
    private bool _stateFinish;
    private bool _banishResult;

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        owner.banishPS.SetActive(true);
        owner.canBanish = true;
        _actualTimer = owner.timeToBanish;
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        owner.banishPS.SetActive(false);
        owner.canBanish = false;
        _stateFinish = false;

        return base.Exit(to);
    }

    public override void UpdateLoop()
    {
        if (!owner.onBanishing) _actualTimer -= Time.deltaTime;

        if (_actualTimer > 0) return;
        _stateFinish = true;
    }

    public override IState ProcessInput()
    {
        if (owner.banished && Transitions.ContainsKey(StateTransitions.ToDeath))
            return Transitions[StateTransitions.ToDeath];

        if (_stateFinish && Transitions.ContainsKey(StateTransitions.ToIdle))
        {
            owner.RestoreLife();
            return Transitions[StateTransitions.ToIdle];
        }

        return this;
    }

}
