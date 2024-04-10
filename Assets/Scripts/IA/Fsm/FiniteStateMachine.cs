using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine 
{
    State _currentState;
    Dictionary<States, State> _allStates = new Dictionary<States, State>();

    public void OnUpdate()
    {
        _currentState?.OnUpdate();
    }

    public void AddState(States name, State state)
    {
        if (!_allStates.ContainsKey(name))
        {
            _allStates.Add(name, state);
            state.fsm = this;
        }
        else
        {
            _allStates[name] = state;
        }
    }

    public void ChangeState(States name)
    {
        _currentState?.OnExit();
        if (_allStates.ContainsKey(name)) _currentState = _allStates[name];
        _currentState?.OnEnter();
    }

    public string GetCurrentState()
    {
        return _currentState.ToString();
    }
}
