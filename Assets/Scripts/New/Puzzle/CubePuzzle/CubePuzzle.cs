using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePuzzle : MonoBehaviour
{
    public static CubePuzzle Instance { get; private set; }
    [SerializeField] private CubeSlot[] slots = new CubeSlot[4];
    public Vector3[] rotations;
    [SerializeField] private string code;
    [SerializeField] private Transform initial, final;
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
        int count = 0;
        string codePlace = "";
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].CubeInSlot == null) break;
            if (slots[i].CubeInSlot.transform.eulerAngles != Vector3.zero) break;
            count++;
            codePlace += slots[i].CubeInSlot.Number.ToString();
        }

        if (count != code.Length || codePlace != code)
        {
            WrongCode();
            return;
        }

        WinPuzzle();
    }

    private void WrongCode()
    {
        print("Mal");
    }

    private void WinPuzzle()
    {
        StartCoroutine(MoveGO());
        foreach (var slot in slots)
        {
            slot.GetComponent<BoxCollider>().enabled = false;
        }
    }

    IEnumerator MoveGO()
    {
        var init = initial;
        float time = 0;
        while (time < 1)
        {
            init.position = Vector3.Lerp(transform.position, final.position, time);
            time+=Time.deltaTime;
            yield return null;
        }
    }
    
    
    
    
}
