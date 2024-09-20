using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleRitual : MonoBehaviour, IBurneable
{
    public GameObject psFire;
    public void OnBurn()
    {
        psFire.SetActive(true);
    }
}
