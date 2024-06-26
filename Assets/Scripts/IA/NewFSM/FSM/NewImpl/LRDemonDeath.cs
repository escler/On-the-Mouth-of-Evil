using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class LRDemonDeath : MonoBaseState
{

    [SerializeField] private DemonLowRange owner;
    public override IState ProcessInput()
    {
        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        owner.GetComponentInChildren<DissolveEnemy>().ActivateDissolve();
    }

    public override void UpdateLoop()
    {
    }
}
