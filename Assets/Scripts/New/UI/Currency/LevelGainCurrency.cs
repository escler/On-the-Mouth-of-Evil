using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGainCurrency : MonoBehaviour
{
    public static LevelGainCurrency Instance {get; private set;}
    public int currency, badEssence, goodEssence, respect;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
