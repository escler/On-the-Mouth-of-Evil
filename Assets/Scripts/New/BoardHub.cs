using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHub : MonoBehaviour
{
    [SerializeField] private GameObject mission1, mission2;
    private void Awake()
    {
        CheckProgress();
    }

    private void CheckProgress()
    {
        var mission1Complete = PlayerPrefs.GetInt("Mission1Complete");
        var mission2Complete = PlayerPrefs.GetInt("Mission2Complete");
        
        mission1.SetActive(mission1Complete == 1);
        mission2.SetActive(mission2Complete == 1);
    }
}
