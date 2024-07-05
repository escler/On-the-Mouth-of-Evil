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
    public bool bossKilled;

    private void Start()
    {
        ObjetivesUI.Instance.AddText(0,"Living Room");
        ObjetivesUI.Instance.AddText(1,"Garage Room");
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
        SceneManager.LoadScene("Lose");
    }

    public void CheckWin()
    {
        if (!bossKilled) return;

        SceneManager.LoadScene("Win");
    }
}
