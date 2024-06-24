using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class LRDemonBanishState : MonoBaseState
{
    [SerializeField] DemonLowRange owner;
    public override IState ProcessInput()
    {

        return this;
    }
    
    public override void UpdateLoop()
    {
        throw new System.NotImplementedException();
    }

}
