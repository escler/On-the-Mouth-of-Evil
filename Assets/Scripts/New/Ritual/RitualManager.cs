using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class RitualManager : MonoBehaviour
{
    public GameObject ritualFloor, ritualBadFloor, stampBlock, stampRelease, psRitual;
    public Candle[] candles;
    public GameObject[] candlesInRitual;
    private int _candlesBurning;
    public int candlesPlaced;
    public bool candleTaked;
    private Candle _actualCandleTaked;
    private Transform cameraPos;
    private RaycastHit _hit;
    public LayerMask layermask;
    public Node ritualNode;
    public GameObject floor;
    public List<ParticleSystem> psCrater;
    public GameObject floorCrater;
    public Color candleColorGoodPath, candleColorBadPath;
    public Material candleMat;
    public bool altarCompleted;
    public bool candlesBurned;
    public GameObject crater;
    public GameObject[] levitatingItems;
    public GameObject actualItemActive;
    
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
        candleMat.color = candleColorGoodPath;
    }

    public void AltarCompleted()
    {
        altarCompleted = true;
        if (DecisionsHandler.Instance.badPath)
        {
            crater.SetActive(false);
            ritualBadFloor.SetActive(true);
            ritualFloor.SetActive(false);
            candleMat.color = candleColorBadPath;
        }
        else
        {
            crater.SetActive(true);
            candleMat.color = candleColorGoodPath;
            ritualFloor.SetActive(true);
            ritualBadFloor.SetActive(false);
        }
        stampBlock.SetActive(false);
        stampRelease.SetActive(true);
        foreach (var candle in candles)
        {
            candle.canTake = true;
            candle.canShowText = true;
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
        var candle = _actualCandleTaked;
        _actualCandleTaked = null;
        candlesInRitual[candlesPlaced].SetActive(true);
        candleTaked = false;
        candlesPlaced++;
        Inventory.Instance.DropItem(Inventory.Instance.selectedItem, Inventory.Instance.countSelected);
        Destroy(candle.gameObject);
    }

    public void ActivateCraterFloor()
    {
        ritualFloor.SetActive(false);
        floorCrater.GetComponent<Animator>().SetBool("Fall", true);
        foreach (var candle in candlesInRitual)
        {
            candle.SetActive(false);
        }
        foreach (var crater in psCrater)
        {
            crater.Play();
        }
    }

    public void CloseCrater()
    {
        floorCrater.GetComponent<Animator>().SetBool("Fall", false);
        foreach (var crater in psCrater)
        {
            crater.Stop();
        }
    }

    public void CandlesBurned()
    {
        _candlesBurning++;
        candlesBurned = _candlesBurning >= 3;
    }

    public void RitualFinish()
    {
        //floor.SetActive(true);
        //craterFloor.SetActive(false);
        ritualBadFloor.SetActive(false);
        ritualFloor.SetActive(false);
        stampRelease.SetActive(false);


    }
}
