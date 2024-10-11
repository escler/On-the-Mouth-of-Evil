using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseEnemyView : MonoBehaviour
{
    public Animator animator;
    public GameObject PS, PSAppear;

    
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
        ResetVariablesInCross();
    }

    public void ActivateGrabHead()
    {
        ChangeStateAnimation("GrabHead", true);
    }

    public void DisableGrabHead()
    {
        ChangeStateAnimation("GrabHead", false);
    }

    public void DisablePointAttack()
    {
        ChangeStateAnimation("CorduraAttack", false);
    }

    public void ActivateCordura()
    {
        CorduraHandler.Instance.StartCordura();
    }

    public void DisableBlockDoorBool()
    {
        ChangeStateAnimation("BlockDoor", false);
    }

    public void LockDoor()
    {
        HouseEnemy.Instance.actualRoom.BlockDoors();
    }

    public void ResetVariablesInCross()
    {
        ChangeStateAnimation("Spawn", false);
        ChangeStateAnimation("Scare", false);
        ChangeStateAnimation("BibleBurning", false);
        ChangeStateAnimation("GrabHead", false);
        ChangeStateAnimation("CorduraAttack", false);
        ChangeStateAnimation("BlockDoor", false);
        ChangeStateAnimation("Exorcism", false);
    }
}
