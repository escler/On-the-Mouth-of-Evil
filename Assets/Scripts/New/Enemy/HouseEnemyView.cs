using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseEnemyView : MonoBehaviour
{
    public Animator animator;
    public GameObject PS;

    
    public void ChangeStateAnimation(string idleName, bool stateParameter)
    {
        animator.SetBool(idleName, stateParameter);
    }

    public void ActivateSpawnPS()
    {
        PS.SetActive(true);
        ChangeStateAnimation("Spawn", false);
    }

    public void DisableCrossBool()
    {
        ChangeStateAnimation("CrossUsed", false);
    }
}
