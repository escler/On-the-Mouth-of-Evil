using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseEnemyView : MonoBehaviour
{
    public Animator animator;
    public GameObject PS;

    
    public void ChangeStateAnimation(string stateName, bool stateParameter)
    {
        animator.SetBool(stateName, stateParameter);
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

    public void ActivateGrabHead()
    {
        ChangeStateAnimation("GrabHead", true);
    }

    public void DisableGrabHead()
    {
        ChangeStateAnimation("GrabHead", false);
    }
}
