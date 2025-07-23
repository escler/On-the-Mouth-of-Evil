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
        StartCoroutine(FireRestNest());
    }

    IEnumerator FireRestNest()
    {
        int count = 1;

        for (int i = count; i < 7; i++)
        {
            nests[i].transform.GetChild(0).gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(3f);

        for (int i = count; i < nests.Length; i++)
        {
            nests[i].transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void DisableNest()
    {
        foreach (var n in nests)
        {
            n.gameObject.SetActive(false);
        }
    }
}
