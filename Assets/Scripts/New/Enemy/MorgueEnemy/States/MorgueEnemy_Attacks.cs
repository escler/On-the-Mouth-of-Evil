using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class MorgueEnemy_Attacks : MonoBaseState
{
    public override void UpdateLoop()
    {
    }

    public override IState ProcessInput()
    {
        return this;
    }
}
