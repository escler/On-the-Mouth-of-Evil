using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RitualManager : MonoBehaviour
{
    public GameObject ritualFloor, chains;
    public Candle[] candles;
    public GameObject[] candlesInRitual;
    private int _candlesPlaced;
    public bool candleTaked;
    private Candle _actualCandleTaked;
    private Transform cameraPos;
    private RaycastHit _hit;
    public LayerMask layermask;
    
    public static RitualManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        cameraPos = PlayerHandler.Instance.cameraPos;
    }

    public void AltarCompleted()
    {
        ritualFloor.SetActive(true);
        chains.SetActive(false);
        foreach (var candle in candles)
        {
            candle.canTake = true;
        }
    }

    public void TakeCandle(Candle candle)
    {
        _actualCandleTaked = candle;
    }

    public void UnassignCandle()
    {
        _actualCandleTaked = null;
    }

    public void CheckCandleFloor()
    {
        if (_actualCandleTaked == null) return;
        candlesInRitual[_candlesPlaced].SetActive(true);
        candleTaked = false;
        _candlesPlaced++;
        Destroy(_actualCandleTaked.gameObject);
    }
}
