using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleRitual : MonoBehaviour, IBurneable
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
}
