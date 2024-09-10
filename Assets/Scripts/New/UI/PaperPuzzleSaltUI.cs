using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPuzzleSaltUI : MonoBehaviour
{
    public GameObject paper;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) paper.SetActive(!paper.activeSelf);
    }
}
