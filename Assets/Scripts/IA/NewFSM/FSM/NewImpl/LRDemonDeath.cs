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
        //Muerte
        owner.gameObject.SetActive(false);
    }

    public override void UpdateLoop()
    {
    }
}
