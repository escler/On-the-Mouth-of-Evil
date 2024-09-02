using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperMission1 : Mission, IInteractable
{
    public string interactableText;
    public void OnInteract()
    {
        CanvasManager.Instance.missionLevelHouse.SetActive(true);
        PlayerHandler.Instance.actualMission = this;
        gameObject.SetActive(false);
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public string ShowText()
    {
        return interactableText;
    }
}
