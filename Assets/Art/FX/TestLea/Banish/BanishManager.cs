using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BanishManager : MonoBehaviour
{ 
    public static BanishManager Instance { get; set; }

    private List<GameObject> linesActives = new List<GameObject>();
    public GameObject lineRendererGO;
    private int _amounTotal, _healthAmount;

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

    public void BanishStart(IEnumerable<IBanishable> entities)
    {
        _amounTotal = AmountOfEnergy(entities).Item1;
        _healthAmount = AmountOfEnergy(entities).Item2;
        TypeManager.Instance.onResult += OnResultOfBanish;
    }

    void OnResultOfBanish()
    {
        TypeManager.Instance.onResult -= OnResultOfBanish;
        if (!TypeManager.Instance.ResultOfType()) return;
        
        Player.Instance.playerEnergyHandler.ModifiedEnergy(_amounTotal);
        Player.Instance.playerLifeHandler.AddHealth(_healthAmount);
        
    }

    public void DeleteLines()
    {
        foreach (var line in linesActives)
        {
            Destroy(line);
        }
    }

    Tuple<int,int> AmountOfEnergy(IEnumerable<IBanishable> entities) //IA2-P1
    {
        return entities.Select(x => (Enemy)x).Where(x => x != null).Aggregate(Tuple.Create(0,0), (acum, current) =>
        {
            current.StartBanish();
            return Tuple.Create(acum.Item1 + current.banishAmount, acum.Item2 + 1);
        });
    }
}
