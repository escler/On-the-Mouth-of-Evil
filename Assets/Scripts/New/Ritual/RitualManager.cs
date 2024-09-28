using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RitualManager : MonoBehaviour
{
    public GameObject ritualFloor, stampBlock, stampRelease, psRitual;
    public Candle[] candles;
    public GameObject[] candlesInRitual;
    private int _candlesPlaced, _candlesBurning;
    public bool candleTaked;
    private Candle _actualCandleTaked;
    private Transform cameraPos;
    private RaycastHit _hit;
    public LayerMask layermask;
    public Node ritualNode;
    public GameObject floor, craterFloor;
    
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
        stampBlock.SetActive(false);
        stampRelease.SetActive(true);
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
        Inventory.Instance.DropItem(Inventory.Instance.selectedItem);

    }

    public void ActivateCraterFloor()
    {
        floor.SetActive(false);
        craterFloor.SetActive(true);
    }

    public void CandlesBurned()
    {
        _candlesBurning++;
        if (_candlesBurning >= candlesInRitual.Length)
        {
            psRitual.SetActive(true);
            HouseEnemy.Instance.RitualReady(ritualNode);
        }
    }
}
