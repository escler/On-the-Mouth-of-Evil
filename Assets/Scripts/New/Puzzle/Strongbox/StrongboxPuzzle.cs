using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongboxPuzzle : MonoBehaviour
{
    public static StrongboxPuzzle Instance { get; private set; }
    [SerializeField] private StrongboxWheel[] wheels = new StrongboxWheel[3];
    [SerializeField] private int correctCode;
    [SerializeField] private StrongboxHandle Handle;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void CheckCode()
    {
        var code = "";

        foreach (var w in wheels)
        {
            code += w.number.ToString();
        }

        if (code == correctCode.ToString())
        {
            print("Gane");
        }
        else print("Perdi");
    }
}
