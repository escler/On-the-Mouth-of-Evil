using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToLobby : MonoBehaviour, IInteractable
{
    public string interactDescription;

    public void OnInteractItem()
    {
        GameManagerNew.Instance.LoadSceneWithDelay("Hub", 0);
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public void OnInteractWithObject()
    {
        
    }

    public string ShowText()
    {
        return interactDescription;
    }

    public bool CanShowText()
    {
        return true;
    }
}
