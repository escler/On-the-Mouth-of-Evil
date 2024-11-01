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
    private int _candlesPlaced, _candlesBurning;
    public bool candleTaked;
    private Candle _actualCandleTaked;
    private Transform cameraPos;
    private RaycastHit _hit;
    public LayerMask layermask;
    public Node ritualNode;
    public GameObject floor;
    public List<ParticleSystem> psCrater;
    public GameObject floorCrater;
    public Color candleColorBadPath;
    public Material candleMat;
    
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
        if (TarotCardPuzzle.Instance.BadPathTaked)
        {
            ritualBadFloor.SetActive(true);
            candleMat.color = candleColorBadPath;
        }
        else ritualFloor.SetActive(true);
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
        candlesInRitual[_candlesPlaced].SetActive(true);
        candleTaked = false;
        _candlesPlaced++;
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
        if (_candlesBurning >= candlesInRitual.Length)
        {
            psRitual.SetActive(true);
        }
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
