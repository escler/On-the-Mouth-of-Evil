using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class IllusionDemon_ThrowObjects : MonoBaseState
{
    [SerializeField] private IllusionDemon owner;
    private float _actualTime;
    private ThrowItem _item;
    private bool _stateFinish;

    public override IState ProcessInput()
    {
        if (owner.canBanish && Transitions.ContainsKey(StateTransitions.ToBanish))
            return Transitions[StateTransitions.ToBanish];
        
        if (_stateFinish && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];

        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        _actualTime = 3;
        _item = ThrowManager.Instance.GetItem();
        owner.actualItem = _item;
        owner.Anim.moveObject = true;
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        owner.Anim.moveObject = false;
        owner.Anim.throwObject = false;
        _item = null;
        _stateFinish = false;
        return base.Exit(to);
    }

    public override void UpdateLoop()
    {
        if (_item == null || _item._callBackHit) _stateFinish = true;
    }

}
