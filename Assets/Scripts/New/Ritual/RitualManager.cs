using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class RitualManager : MonoBehaviour
{
    public GameObject ritualFloor, ritualBadFloor, psRitual;
    public Transform[] candlesInRitual;
    private int _candlesBurning;
    public int candlesPlaced;
    public bool candleTaked;
    private Candle _actualCandleTaked;
    public Candle firstCandlePlaced;
    private Transform cameraPos;
    private RaycastHit _hit;
    public LayerMask layermask;
    public Node ritualNode;
    public GameObject floor;
    public List<ParticleSystem> psCrater;
    public VisualEffect heat;
    public GameObject floorCrater;
    public bool altarCompleted;
    public bool candlesBurned;
    public GameObject crater;
    public GameObject[] levitatingItems;
    public GameObject actualItemActive;
    public GameObject circles;
    public GameObject godRayVFX;
   
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
        return;
        altarCompleted = true;
        circles.SetActive(true);
        if (DecisionsHandler.Instance.badPath)
        {
            //sonidoPuzzleSalt
            crater.SetActive(false);
            ritualBadFloor.SetActive(true);
            ritualFloor.SetActive(false);
        }
        else
        {
            //sonidoPuzzleSalt
            heat.Stop();
            crater.SetActive(true);
            ritualFloor.SetActive(true);
            ritualBadFloor.SetActive(false);
        }
    }

    public void CandlePlaced(Candle candle)
    {
        if (candlesPlaced <= 0)
        {
            firstCandlePlaced = candle;
            if (candle.badCandle) ritualBadFloor.SetActive(true);
            else ritualFloor.SetActive(true);
        }
        candlesPlaced++;
    }

    public void RemoveCandle(Candle candle)
    {
        candlesPlaced--;
        if (candlesPlaced > 0) return;
        firstCandlePlaced = null;
        if (candle.badCandle) ritualBadFloor.SetActive(false);
        else ritualFloor.SetActive(false);
    }

    public void ActivateCraterFloor()
    {
        ritualFloor.SetActive(false);
        floorCrater.GetComponent<Animator>().SetBool("Fall", true);
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
        candlesPlaced++;
        candlesBurned = _candlesBurning >= 3;
    }

    public void RitualFinish()
    {
        //floor.SetActive(true);
        //craterFloor.SetActive(false);
        ritualBadFloor.SetActive(false);
        ritualFloor.SetActive(false);
    }
}
