using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitHub : MonoBehaviour, IInteractable
{
    public string interactDescription;
    private int _count;
    private int _itemNeeded = 4;
    private AudioSource audioSource;
    public string soundName;

    public void OnInteractItem()
    {
        _count = 0;
        if (PlayerHandler.Instance.actualMission == null) return;

        if (!TutorialHub.Instance.TutorialCompleted) TutorialHub.Instance.exithub = true;

        MusicManager.Instance.PlaySound(soundName, false);
        GameManagerNew.Instance.LoadSceneWithDelay(PlayerHandler.Instance.actualMission.misionName, 3f);
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
