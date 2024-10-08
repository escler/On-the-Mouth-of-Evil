using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleRitual : MonoBehaviour, IBurneable, IInteractable
{
    public GameObject psFire;
    private bool burning;
    public void OnBurn()
    {
        if (burning) return;
        psFire.SetActive(true);
        burning = true;
        RitualManager.Instance.CandlesBurned();
    }

    public void OnInteractItem()
    {
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public void OnInteractWithObject()
    {
        
    }

    public string ShowText()
    {
        return "";
    }

    public bool CanShowText()
    {
        return false;
    }
}
