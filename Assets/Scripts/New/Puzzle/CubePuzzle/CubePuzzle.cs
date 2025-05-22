using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CubePuzzle : MonoBehaviour
{
    public static CubePuzzle Instance { get; private set; }
    [SerializeField] private CubeSlot[] slots = new CubeSlot[4];
    public Vector3[] rotations;
    [SerializeField] private string code;
    [SerializeField] private Transform initial, final;
    private Vector3 orientation = new Vector3(0, -180, 0);
    [SerializeField] private GameObject key;
    private bool _corInitialized;
    [SerializeField] private GameObject goodAura;
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
            var slot = slots[i];
            if (slot.CubeInSlot.transform.up != Vector3.up) break;
            if (slot.CubeInSlot.transform.forward != slot.transform.forward ) break;
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
        if(!_corInitialized)StartCoroutine(MoveGO());
        goodAura.SetActive(true);
        key.SetActive(true);
        key.GetComponent<KeyGood>().ChangeLight(true);
        foreach (var slot in slots)
        {
            slot.GetComponent<BoxCollider>().enabled = false;
        }
    }

    IEnumerator MoveGO()
    {
        _corInitialized = true;
        initial.GetComponent<AudioSource>().Play();
        var init = initial;
        float time = 0;
        while (time < 1)
        {
            initial.position = Vector3.Lerp(init.position, final.position, time);
            time+=Time.deltaTime;
            yield return null;
        }
    }
    
    
    
    
}
