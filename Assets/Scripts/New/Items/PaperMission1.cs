using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperMission1 : Mission
{
    public string interactableText;
    private bool active;

    private void Start()
    {
        active = CanvasManager.Instance.missionLevelHouse.activeInHierarchy;
    }

    public override void OnGrabMission()
    {
        base.OnGrabMission();
        PlayerHandler.Instance.actualMission = this;
    }

    public void OnInteract()
    {
        //CanvasManager.Instance.missionLevelHouse.SetActive(true);
    }

    public override void OnInteract(bool hit, RaycastHit i)
    {
        CanvasManager.Instance.missionLevelHouse.SetActive(!active);
        active = CanvasManager.Instance.missionLevelHouse.activeInHierarchy;
        Inventory.Instance.cantSwitch = active;
    }

    public string ShowText()
    {
        return interactableText;
    }
}
