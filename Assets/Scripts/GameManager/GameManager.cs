using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public SpatialGrid activeSpatialGrid;
    public ZoneManager activeZoneManager;

    private void Start()
    {
        ListDemonsUI.Instance.AddText(0,"Living Room");
        ListDemonsUI.Instance.AddText(1,"Garage Room");
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void GameLose()
    {
        SceneManager.LoadScene(0);
    }
}
