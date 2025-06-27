using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestHandler : MonoBehaviour
{
    public static NestHandler Instance { get; private set; }
    
    public GameObject[] nests;
    [SerializeReference] private Transform liquidPos;
    
    public Transform LiquidPos => liquidPos;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    public void FireMainNest()
    {
        nests[0].transform.GetChild(0).gameObject.SetActive(true);
        GoodRitual.Instance.nestOnFire = true;
    }

    public void FireRestNest()
    {
        for (int i = 1; i < nests.Length; i++)
        {
            nests[i].transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
