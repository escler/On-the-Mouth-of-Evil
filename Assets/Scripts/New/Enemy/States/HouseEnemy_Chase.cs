using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class HouseEnemy_Chase : MonoBaseState
{
    [SerializeField] private HouseEnemy owner;
    public override void UpdateLoop()
    {
        
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
    }

    public override IState ProcessInput()
    {
        throw new System.NotImplementedException();
    }
}
