using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : Controller
{
    public override Vector3 GetMovementInput()
    {
        Vector3 modifiedMoveDir = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
        return modifiedMoveDir.normalized;
    }
}