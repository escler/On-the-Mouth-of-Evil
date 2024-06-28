using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanishManager : MonoBehaviour
{ 
    public static BanishManager Instance { get; set; }

    private List<GameObject> linesActives = new List<GameObject>();
    public GameObject lineRendererGO;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void CreateNewBanishLine(Vector3 enemyPos)
    {
        var newLine = Instantiate(lineRendererGO);
        var lineRenderer = newLine.GetComponent<LineRenderer>();
        
        lineRenderer.SetPosition(0,enemyPos);
        lineRenderer.SetPosition(1,Player.Instance.transform.position);
        linesActives.Add(newLine);
    }

    public void DeleteLines()
    {
        foreach (var line in linesActives)
        {
            Destroy(line);
        }
    }
}
